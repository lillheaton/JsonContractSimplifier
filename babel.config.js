/* eslint-env node */

module.exports = function(api) {
  api.cache(true)

  return {
    presets: ['@babel/env'],

    plugins: ['@babel/plugin-proposal-object-rest-spread']
  }
}
