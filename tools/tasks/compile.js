import exec from './exec'

export default function compile(settings) {
  console.log(
    `dotnet build ${settings.sourcePath} -c ${settings.target} --no-restore`
  )
  return exec(
    `dotnet build ${settings.sourcePath} -c ${settings.target} --no-restore`
  )
}
