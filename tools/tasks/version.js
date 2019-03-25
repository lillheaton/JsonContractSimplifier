import chalk from 'chalk'
import fs from 'fs'
import { exec } from 'child-process-promise'
import { log, logError } from 'simple-make/lib/logUtils'

function gitCommit() {
  const git = 'git log -1 --pretty=format:%H'
  return exec(git)
    .then(function(result) {
      return result.stdout
    })
    .fail(function(err) {
      logError(chalk.red(err.stdout))
    })
}

export default function version({ versionInfo, assemblyInfoFilePath }) {
  return gitCommit().then(commit => {
    log(`Writing ${assemblyInfoFilePath}\n`)

    const {
      version,
      fileVersion,
      informationalVersion,
      description,
      productName,
      copyright
    } = versionInfo

    const trademark = commit
    const fileInfo = `using System;
using System.Reflection;
[assembly: AssemblyDescription("${description}")]
[assembly: AssemblyTitle("${productName}")]
[assembly: AssemblyProduct("${productName}")]
[assembly: AssemblyCopyright("${copyright}")]
[assembly: AssemblyTrademark("${trademark}")]
[assembly: AssemblyVersion("${version}")]
[assembly: AssemblyFileVersion("${fileVersion}")]
[assembly: AssemblyInformationalVersion("${informationalVersion}")]
[assembly: CLSCompliant(false)]`

    return new Promise((resolve, reject) => {
      fs.writeFile(assemblyInfoFilePath, fileInfo, err => {
        if (err) {
          reject(err)
        } else {
          resolve()
        }
      })
    })
  })
}
