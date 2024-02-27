<template>
  <div v-if="links && links.length" class="widget">
    <div class="widget-header">
      <span>友情链接</span>
      <span class="slot"
        ><nuxt-link to="/links">查看更多&gt;&gt;</nuxt-link></span
      >
    </div>
    <div class="widget-content">
      <ul class="links">
        <li v-for="link in links" :key="link.linkId" class="link">
          <div class="link-logo fldiv">
            <img v-if="link.logo" :src="link.logo" class="flimg" />
            <img v-if="!link.logo" src="~/assets/images/net.png" />
          </div>
          <div class="link-content">
            <a
              :href="link.url"
              :title="link.title"
              class="link-title"
              target="_blank"
              >{{ link.title }}</a
            >
            <p class="link-summary">
              {{ link.summary }}
            </p>
          </div>
        </li>
      </ul>
    </div>
  </div>
</template>

<script>
export default {
  name: 'FriendLinks',
  props: {
    links: {
      type: Array,
      default() {
        return null
      },
    },
  },
}
</script>

<style scoped lang="scss">
.fldiv {
  position: relative;
}
.flimg {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}
</style>
