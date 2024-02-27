<template>
  <div class="widget">
    <div class="widget-header">
      <div>
        <span>粉丝</span>
        <span class="count">{{ user.fansCount }}</span>
      </div>
      <div class="slot">
        <a @click="showMore">更多</a>
      </div>
    </div>
    <div class="widget-content">
      <div v-if="fansList && fansList.length">
        <user-follow-list :users="fansList" @onFollowed="onFollowed" />
      </div>
      <div v-else class="widget-tips">没有更多内容了</div>
    </div>

    <el-dialog
      title="粉丝"
      :visible.sync="showFansDialog"
      custom-class="my-dialog"
    >
      <div v-loading="fansDialogLoading">
        <load-more
          v-if="fansPage"
          ref="commentsLoadMore"
          v-slot="{ results }"
          :init-data="fansPage"
          :params="{ limit: 10 }"
          :url="'/api/follow/fans/' + user.id"
        >
          <user-follow-list :users="results" @onFollowed="onFollowed" />
        </load-more>
        <div v-else>没数据</div>
      </div>
    </el-dialog>
  </div>
</template>

<script>
export default {
  props: {
    user: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      fansList: [],
      showFansDialog: false,
      fansDialogLoading: false,
      fansPage: null,
    }
  },
  mounted() {
    this.loadData()
  },
  methods: {
    async loadData() {
      const data = await this.$axios.get('/api/follow/fans/' + this.user.id)
      this.fansList = data.results
    },
    async onFollowed(userId, followed) {
      await this.loadData()
    },
    async showMore() {
      this.showFansDialog = true
      this.fansDialogLoading = true
      try {
        this.fansPage = await this.$axios.get(
          '/api/follow/fans/' + this.user.id,
          {
            params: {
              limit: 10,
            },
          }
        )
      } catch (e) {
        // this.$message.error(e.message || e)
        this.$showErrorMsg(e.response.data.error)
      } finally {
        this.fansDialogLoading = false
      }
    },
  },
}
</script>

<style lang="scss"></style>
