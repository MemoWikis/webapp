module.exports = {
    devServer: {
      proxy: {
          target: 'http://localhost:26590',
      }
    }
  }