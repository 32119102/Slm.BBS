class LuteHelper {
  luteScript = '/lute.min.js'

  MarkdownStr(markdown, callback) {
    if (!process.client) {
      return
    }
    const me = this
    if (!window.lute) {
      me.addScript(this.luteScript, function () {
        // eslint-disable-next-line no-undef
        const lute = Lute.New()
        // const renderers = {
        // renderText: (node, entering) => {
        //   if (entering) {
        //     // eslint-disable-next-line no-undef
        //     return [node.Text() + '', Lute.WalkContinue]
        //   }
        //   // eslint-disable-next-line no-undef
        //   return ['', Lute.WalkContinue]
        // },
        // }

        // lute.SetJSRenderers({
        //   renderers: {
        //     // Md2HTML: renderers,
        //   },
        // })
        // 设置是否将软换行（\n）渲染为硬换行（<br />）
        lute.SetSoftBreak2HardBreak(true)
        // AutoSpace 设置是否对普通文本中的中西文间自动插入空格。
        // https://github.com/sparanoid/chinese-copywriting-guidelines
        lute.SetAutoSpace(true)
        // 设置是否打开“目录”支持
        // lute.SetToC(true)
        // 设置是否对标题生成链接锚点
        // lute.SetHeadingAnchor(true)
        // 设置是否打开“GFM 任务列表项”支持
        lute.SetGFMTaskListItem(true)
        // 设置是否启用 XSS 安全过滤 https://github.com/88250/lute/issues/51
        lute.SetSanitize(true)
        // 设置图片懒加载时使用的图片路径，配置该字段后将启用图片懒加载。
        // 图片 src 的值会复制给新属性 data-src，然后使用该参数值作为 src 的值 https://github.com/88250/lute/issues/55
        lute.SetImageLazyLoading('/loading.png')

        window.lute = lute
        if (callback) {
          const result = window.lute.MarkdownStr('', markdown)
          const ret = me.replaceTarget(result)
          callback(ret)
        }
      })
    } else {
      const result = window.lute.MarkdownStr('', markdown)
      const ret = me.replaceTarget(result)
      return ret
    }
  }

  replaceTarget(html) {
    let _html = html
    _html = _html.replace(/<a[^>]+>/g, function (a) {
      if (!/\starget\s*=/.test(a)) {
        return a.replace(/^<a\s/, '<a target="_blank" ')
      }
      return a
    })
    return _html
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

export default new LuteHelper()
