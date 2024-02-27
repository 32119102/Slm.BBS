<template>
  <div class="index-container">
    <div v-if="systeminfo" class="widget">
      <div class="widget-header">服务器信息</div>
      <div class="widget-content">
        <div>系统版本: {{ systeminfo.osVersion }}</div>
        <div>系统架构: {{ systeminfo.arch }}</div>
        <div>线程数量: {{ systeminfo.processorCount }}</div>
        <div>程序物理路径: {{ systeminfo.applicationBasePath }}</div>
        <div>应用名称: {{ systeminfo.applicationName }}</div>
        <div>应用版本: {{ systeminfo.applicationVersion }}</div>
        <div>框架版本: {{ systeminfo.runtimeFramework }}</div>
        <div>服务器IP: {{ systeminfo.host }}</div>
        <div>服务器端口: {{ systeminfo.port }}</div>
        <div>服务器名称: {{ systeminfo.machineName }}</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      systeminfo: null,
    };
  },
  mounted() {
    this.load();
  },
  methods: {
    async load() {
      this.systeminfo = await this.axios.get("/api/common/systeminfo");
    },
  },
};
</script>

<style lang="scss" scoped>
.index-container {
  margin: 20px;
  font-size: 14px;
  font-family: inherit;
  color: inherit;
  font-weight: 500;
  line-height: 1.8;

  .widget-content {
    margin-top: 0px;
  }

  .index-title {
    font-size: 24px;
    font-weight: 700;
  }

  .card {
    padding: 10px;
    position: relative;
    display: flex;
    flex-direction: column;
    min-width: 0;
    word-wrap: break-word;
    background-clip: border-box;
    background-color: #fff;
    border: 1px solid rgba(0, 0, 0, 0.125);
    border-radius: 0.25rem;
  }
}
</style>
