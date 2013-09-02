Properties {
  $projectName ="TeamcityNotifier"
  
  $baseDir = Resolve-Path ..
  $sourceDir = "$baseDir\source"
  $scriptsDir = "$baseDir\scripts"
  $packagesDir = "$sourceDir\packages"
  
  $slnFile = "$sourceDir\$projectName.sln"
    
  $versionFile = "$baseDir\Version.txt"

  $assemblyInfoFileName = "AssemblyInfo.cs"

  $xunitRunner = "$packagesDir\NUnit.Runners.*\tools\nunit-console-x86.exe"
  $mspecConsole = "$packagesDir\Machine.Specifications.*\tools\mspec-clr4.exe"
  
  $teamcity = $false
  $publish = $false
  $parallelBuild = $true
  
  $buildConfig = "Release"
  $buildNumber = 0
  
  $projects = CoreProjects
}

FormatTaskName (("-"*70) + [Environment]::NewLine + "[{0}]"  + [Environment]::NewLine + ("-"*70))

Task default –depends Clean, WriteAssemblyInfo, Build, CheckHintPaths, Test, ResetAssemblyInfo

Task Clean { 

    Get-Childitem $sourceDir -Include bin, obj -Recurse | 
    Where {$_.psIsContainer -eq $true} | 
    Foreach-Object { 
        Write-Host "deleting" $_.fullname
        Remove-Item $_.fullname -Force -Recurse -ErrorAction SilentlyContinue
    }
}

Task WriteAssemblyInfo -precondition { return $publish } -depends Clean {
    $assemblyVersionPattern = 'AssemblyVersionAttribute\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
	$fileVersionPattern = 'AssemblyFileVersionAttribute\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    
    $majorMinor = (Get-Content $versionFile).split(".")
    $major = $majorMinor[0]
    $minor = $majorMinor[1]

    $projects | 
    Foreach-Object { 
		$project = $_.fullname

        $assemblyInfoFile = "$project\Properties\$assemblyInfoFileName"
           
        $assemblyVersion = 'AssemblyVersionAttribute("' + (VersionNumber $major $minor 0 0 ) + '")'
        $fileVersion = 'AssemblyFileVersionAttribute("' + (VersionNumber $major $minor $buildNumber 0 ) + '")'
        
        Write-Host "updating" $assemblyInfoFile "with Assembly version:" (VersionNumber $major $minor 0 0 ) "Assembly file version:" (VersionNumber $major $minor $buildNumber 0)

        (Get-Content $assemblyInfoFile) | ForEach-Object {
            % {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
            % {$_ -replace $fileVersionPattern, $fileVersion }
        } | Set-Content $assemblyInfoFile
    }
}

Task Build -depends Clean, WriteAssemblyInfo {
    Write-Host "building" $slnFile
    
    if($parallelBuild){
        $parallelBuildParam = "/m"
        $maxCpuCount = [Environment]::GetEnvironmentVariable("MAX_CPU_COUNT","User")
        if($maxCpuCount){
            $parallelBuildParam += ":$maxCpuCount"
        }
    }

    Write-Host "building using msbuild" $slnFile "/p:Configuration=$buildConfig" "/verbosity:minimal" "/fileLogger" "/fileLoggerParameters:LogFile=$baseDir/msbuild.log" $parallelBuildParam

    Exec { msbuild $slnFile "/p:Configuration=$buildConfig" "/verbosity:minimal" "/fileLogger" "/fileLoggerParameters:LogFile=$baseDir/msbuild.log" $parallelBuildParam}
    
}

Task CheckHintPaths -depends Clean, WriteAssemblyInfo, Build {

    $exclude = "System*|Microsoft*"
    $startPatterns = @("..\packages\")
    
    $ns = @{ defaultNamespace = "http://schemas.microsoft.com/developer/msbuild/2003" }

    Get-ChildItem $sourceDir -Include "*.csproj" -Recurse | Foreach-Object{
        $projectFile = $_.fullname
        $projectDir = $_.fullname.replace($_.name, "")
        
        Write-Host "checking" $projectFile
        
        try{
            Select-Xml $projectFile -XPath "//defaultNamespace:Reference" -Namespace $ns | 
            Select -ExpandProperty Node | 
            Foreach-Object {
                if($_.Include -match $exclude){
                    return
                }
                
                $assemblyName = $_.Include.split(",")[0]
                $hintPath = $_.HintPath
                
                if(!$hintPath){
                    throw "The HintPath of the `"$assemblyName`" reference is missing."
                }

                $startOk = $startPatterns | Foreach-Object{
                    if($hintPath.StartsWith($_)){
                        return $true
                    }
                }
                
                if(!$startOk){
                    throw "The HintPath of the `"$assemblyName`" reference doesn't match one of the start patterns."
                }
                
                if(!$hintPath.Contains($assemblyName)){
                    throw "The HintPath of the `"$assemblyName`" reference doesn't contain the assembly name."
                }

                if(!(Test-Path($projectDir+$hintPath))){
                    throw "The HintPath of the `"$assemblyName`" reference is incorrect. The referenced file doesn't exist."
                }
            }
        }catch{
            throw "Reference-Error: " + $_
        }

    } 

}

Task Test -depends Clean, Build, CheckHintPaths {
    RunUnitTest
    RunMSpecTest
}

Task ResetAssemblyInfo -precondition { return $publish -and !$teamcity } -depends Clean, WriteAssemblyInfo, Build, CheckHintPaths, Test {
    $projects | 
    Foreach-Object { 
       $assemblyInfoFile = $_.fullname + "\Properties\$assemblyInfoFileName"
       Write-Host "resetting" $assemblyInfoFile
       exec { cmd /c "git checkout $assemblyInfoFile" }
    }
    
}

Function RunUnitTest {
    #get newest xunit runner
    $newestRunner = GetNewest($xunitRunner)
    Write-Host "using" $newestRunner
    
    Get-Childitem $sourceDir -Recurse |
    Where{$_.fullname -like "*.Test\bin\$buildConfig\*Test.dll" } |
    Foreach-Object {
        $testFile = $_.fullname
        Write-Host "testing" $testFile 
        exec { cmd /c "$newestRunner $testFile" }
    }
}

Function RunMSpecTest {
    #get newest mspec runner
    $newestConsole = GetNewest($mspecConsole)
    Write-Host "using" $newestConsole
    
    Get-Childitem $sourceDir -Recurse |
    Where{$_.fullname -like "*.Specification\bin\$buildConfig\*Specification.dll" } |
    Foreach-Object {
        $testFile = $_.fullname
        
        if($teamcity){
            $htmlPath = $binariesDir +"\"+ $_.name + ".html"
            $additionalParams = "--html $htmlPath --teamcity"
        }
 	
        Write-Host "testing" $testFile 
        exec { cmd /c "$newestConsole $additionalParams $testFile" }
					
    }
}

Function CoreProjects {
    return Get-Childitem $sourceDir | 
    Where{$_.psIsContainer -eq $true -and 
    $_.name -like "$projectName.*" -and 
    $_.name -notlike "$projectName.*.Test" -and 
    $_.name -notlike "$projectName.*.Specification" -and 
    $_.name -notlike "$projectName.*.Sample" -and 
    $_.name -notlike "$projectName.*.Performance" -and 
    $_.name -notlike "\.*"}
}

Function VersionNumber([string] $n1,[string] $n2,[string] $n3,[string] $n4){
    "$n1.$n2.$n3.$n4"
}

Function GetNewest($path){
    $firstPart = $path.split("*")[0]
    $secondPart = $path.split("*")[1]
	
	$highestVersion = [version] "0.0"
    Get-Item $path | Foreach-Object {
	    $p = GetRidOfPrereleaseInfo($_.fullname.Replace($firstPart, "").Replace($secondPart, ""))
        $currentVersion = [version] $p
        if($currentVersion.CompareTo($highestVersion) -gt 0){
            $highestVersion = $currentVersion
            $newest = $_.fullname
        }
    }
    
    return $newest
}

Function GetRidOfPrereleaseInfo($path) {

    if($path.Contains("-")) {
	    $newPath = $path.SubString(0, $path.IndexOf("-"))
		Write-Host "cut away pre-release version info " $path " -> " $newPath
		return $newPath
	}
	
	return $path
}