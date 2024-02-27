// import qs from 'qs'

export default function ({ $axios, app }) {
  $axios.onRequest((config) => {
    config.headers.common['X-Client'] = 'bbs-site'
    config.headers.post['Content-Type'] = 'application/json'
    const userToken = app.$cookies.get('userToken')
    if (userToken) {
      config.headers.Authorization = 'Bearer ' + userToken
    }
    // config.transformRequest = [
    //   // function (data) {
    //   //   if (process.client && data instanceof FormData) {
    //   //     // 如果是FormData就不转换
    //   //     return data
    //   //   }
    //   //   data = qs.stringify(data)
    //   //   return data
    //   // },
    // ]
    // console.log(config)
  })

  $axios.onResponse((response) => {
    // console.log('repsonse', response.config.url, response.status, response.data)
    // if (response.status === 401) {
    //   // 跳转登录页
    //   this.$toSignin()
    // } else
    if (response.status !== 200 && response.status !== 204) {
      return Promise.reject(response)
    }
    return Promise.resolve(response.data)
    // const jsonResult = response.data
    // if (jsonResult.success) {
    //   return Promise.resolve(jsonResult.data)
    // } else {
    //   return Promise.reject(jsonResult)
    // }
  })
}
