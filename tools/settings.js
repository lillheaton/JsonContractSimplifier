import path from 'path'
import moment from 'moment'
import pjson from '../package.json'

const target = process.env.CONFIGURATION || 'Debug'
const buildNumber = process.env.APPVEYOR_BUILD_NUMBER
const appVeyorJobId = process.env.APPVEYOR_JOB_ID
const version = pjson.version
const revision = buildNumber || moment().format('HHmm')
const includeRevision = true
const versionSuffix = 'preview'
const assemblyVersion = includeRevision
  ? `${version}.${revision}`
  : `${version}.0`
const CI = process.env.CI && process.env.CI.toString().toLowerCase() === 'true'

const artifactsPath = path.resolve('./artifacts')
const solutionPath = path.resolve('./EOls.Serialization.sln')
const projectSourcePath = path.resolve('./src')
const cleanPaths = [`${projectSourcePath}/obj`, `${projectSourcePath}/bin`]

const versionInfo = {
  description: 'EOls.Serialization an shorthand for Json.NET',
  productName: 'EOls.Serialization',
  copyright: 'Copyright 2019 Emil Olsson',
  version: assemblyVersion,
  fileVersion: assemblyVersion,
  informationalVersion: assemblyVersion
}

export default {
  appVeyorJobId,
  artifacts: artifactsPath,
  CI,
  cleanPaths,
  slnPath: solutionPath,
  sourcePath: projectSourcePath,
  target,
  version,
  revision,
  includeRevision,
  assemblyVersion,
  taskTimeout: 120000,
  versionSuffix,
  versionInfo,
  assemblyInfoFilePath: './CommonAssemblyInfo.cs'
}
