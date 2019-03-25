import make from 'simple-make/lib/make'
import config from 'simple-make/lib/config'
import { log } from 'simple-make/lib/logUtils'
import {
  compile,
  clean,
  dotnetPack,
  restore,
  setVersion,
  version
} from './tasks'
import settings from './settings'

const args = process.argv.slice(2)
console.log(args)

log('Settings', settings)

config.name = settings.versionInfo.productName
config.taskTimeout = settings.taskTimeout

const tasks = {
  artifacts: ['nuget'],
  compile: [clean, 'restore', compile],
  test: ['compile'],
  version: [version],
  nuget: dotnetPack,
  restore,
  setVersion: () => setVersion(args[1]),
  default: 'test',
  ci: 'version default artifacts'
}

make({ tasks, settings })
