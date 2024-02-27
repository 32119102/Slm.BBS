class ShowDownHelper {
  showDownScript =
    'https://cdnjs.cloudflare.com/ajax/libs/showdown/2.1.0/showdown.min.js'

  makeHtml(md) {
    if (!process.client) {
      return
    }
    const me = this
    if (!window.showdown) {
      me.addScript(this.showDownScript, function () {
        // eslint-disable-next-line no-undef
        const converter = new showdown.Converter()
        converter.setOption('tables', true)
        // eslint-disable-next-line no-undef
        // console.log(showdown.getDefaultOptions())
        return converter.makeHtml(md)
      })
    } else {
      // eslint-disable-next-line no-undef
      const converter = new showdown.Converter()
      converter.setOption('tables', true)
      // eslint-disable-next-line no-undef
      // console.log(showdown.getDefaultOptions())
      return converter.makeHtml(md)
    }
  }

  addScript(url, callback) {
    if (!process.client) {
      console.warn('Add script fail, !process.client, ' + url)
      return
    }
    const script = document.createElement('script')
    script.type = 'text/javascript'
    script.src = url
    script.defer = true
    document.body.appendChild(script)
    script.onload = function () {
      if (callback) callback()
    }
  }
}

export default new ShowDownHelper()
