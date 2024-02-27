import axios from "axios";
import VueAxios from "vue-axios";
import { Message } from "element-ui";
import store from "@/store";
import { getToken } from "@/utils/auth";
import qs from "qs";
import Vue from "vue";

const formatFormDataKey = "__formData";
const apiBaseUrl = process.env.VUE_APP_BASE_API;

function isFormData(data) {
  return data && data[formatFormDataKey] === "formData";
}

function toLogin() {
  // 去登录
  store.dispatch("user/resetToken").then(() => {
    location.reload();
  });
}

function getJwtToken() {
  return `Bearer ${getToken()}`;
}

// create an axios instance
const axiosInstance = axios.create({
  baseURL: apiBaseUrl, // url = base url + request url
  timeout: 5000, // request timeout,
});

// 设置form请求
axiosInstance.form = function (url, data, config) {
  if (!data) {
    data = {};
  }
  data[formatFormDataKey] = "formData";
  return this.post(url, data, config);
};

// request interceptor
axiosInstance.interceptors.request.use(
  (config) => {
    if (store.getters.token) {
      config.headers.Authorization = getJwtToken();
    }

    if (config.method === "post" || config.method === "put") {
      config.headers["Content-Type"] = "application/json";
    }

    // 如果是form请求
    if (isFormData(config.data)) {
      delete config.data[formatFormDataKey];
      config.data = qs.stringify(config.data); // 转为formdata数据格式
    }

    return config;
  },
  (error) => Promise.reject(error)
);

// 是否正在刷新的标记
let isRefreshing = false;
// 重试队列，每一项将是一个待执行的函数形式
let requests = [];

// response interceptor
axiosInstance.interceptors.response.use(
  (response) => {
    // console.log(response);
    const res = response.data;
    return Promise.resolve(res);
  },
  (error) => {
    var resp = error.response;
    const config = resp.config;
    var err = resp.data.error;
    var token = getToken();
    // 刷新授权令牌或者重新登录
    if (resp.status === 401) {
      if (token) {
        if (!isRefreshing) {
          isRefreshing = true;
          store
            .dispatch("user/refresh")
            .then(() => {
              config.headers.Authorization = getJwtToken();
              // 已经刷新了token，将所有队列中的请求进行重试
              requests.length > 0 && requests.forEach((cb) => cb());
              requests = [];
              return axios.request(config);
            })
            .catch(() => {
              toLogin();
              return Promise.resolve(resp.data);
            })
            .finally(() => {
              isRefreshing = false;
            });
        } else {
          // 正在刷新token，将返回一个未执行resolve的promise
          return new Promise((resolve) => {
            // 将resolve放进队列，用一个函数形式来保存，等token刷新后直接执行
            requests.push(() => {
              config.headers.Authorization = getJwtToken();
              resolve(axios.request(config));
            });
          });
        }
      } else {
        // 去登录
        toLogin();
        return Promise.resolve(resp.data);
      }
    }

    var errmsg = err && (err.code || "") + "：" + (err.message || "未知异常");
    if (err && err.details) {
      errmsg += "\r\n" + err.details;
    }
    var errmsgArr = errmsg.split("\r\n");
    var newErrs = [];
    errmsgArr.forEach(function (item) {
      if (item) {
        newErrs.push("<p>" + item + "</p>");
      }
    });
    console.log(newErrs);
    if (newErrs && newErrs.length > 0) {
      Message({
        message: newErrs.join(""),
        type: "error",
        duration: 5 * 1000,
        dangerouslyUseHTMLString: true,
      });
    }

    // 已失效，去登录
    if (err && err.code === "Auth.JwtToken.Invalid:20100") {
      toLogin();
      return Promise.resolve(resp.data);
    }

    return Promise.reject(error);
  }
);

// export default axiosInstance
Vue.use(VueAxios, axiosInstance);
