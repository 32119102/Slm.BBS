<template>
  <section v-loading="listLoading" class="page-container">
    <div class="toolbar">
      <el-form :inline="true" :model="filters">
        <!-- <el-form-item>
          <el-input v-model="filters.id" placeholder="编号" />
        </el-form-item> -->
        <el-form-item>
          <el-input v-model="filters.userName" placeholder="用户名" />
        </el-form-item>
        <el-form-item>
          <el-input v-model="filters.title" placeholder="标题" />
        </el-form-item>
        <el-form-item>
          <el-select v-model="filters.recommend" clearable placeholder="是否推荐">
            <el-option label="推荐" value="1" />
            <el-option label="未推荐" value="0" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-select v-model="filters.sticky" clearable placeholder="是否置顶">
            <el-option label="是" value="1" />
            <el-option label="否" value="0" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-select v-model="filters.status" clearable placeholder="请选择状态">
            <el-option label="正常" value="0" />
            <el-option label="删除" value="1" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="list"> 查询 </el-button>
        </el-form-item>
      </el-form>
    </div>

    <div class="page-section topics">
      <div v-for="item in results" :key="item.id" class="topic-item">
        <div class="topic-avatar">
          <avatar :user="item.user" />
        </div>
        <div class="topic-main">
          <div class="topic-header">
            <a :href="('/user/' + item.user.id) | siteUrl" target="_blank" class="topic-nickname">{{
              item.user.nickname
            }}</a>

            <div class="topic-info">
              <span v-if="item.status === 1" style="color: red; font-weight: bold">已删除</span>
              <span v-if="item.recommend" style="color: red; font-weight: bold">已推荐</span>
              <span v-if="item.sticky" class="topic-sticky-icon">置顶</span>
            </div>
          </div>

          <div class="topic-metadata">
            <span class="topic-metadata-item">ID: {{ item.id }}</span>
            <span class="topic-metadata-item">发布于：{{ item.createTime | formatDate }}</span>
            <span class="node topic-metadata-item">{{ item.node ? item.node.name : "" }}</span>
            <span class="topic-metadata-item">
              <el-tag
                v-for="tag in item.tags"
                :key="tag.id"
                type="info"
                size="mini"
                style="margin-right: 12px"
              >
                #{{ tag.name }}
              </el-tag>
            </span>
          </div>

          <div class="topic-title">
            <a :href="('/topic/' + item.id) | siteUrl" target="_blank">{{ item.title }}</a>
          </div>

          <!-- <template v-if="item.type === 0">
            <div class="topic-summary">
              {{ item.summary }}
            </div>
          </template>
          <template v-else>
            <div class="topic-summary">
              {{ item.content }}
            </div>
          </template> -->
          <div class="topic-summary">
            {{ item.summary }}
          </div>

          <ul v-if="item.imageList && item.imageList.length" class="topic-image-list">
            <li v-for="(image, index) in item.imageList" :key="index">
              <a :href="('/topic/' + item.id) | siteUrl" target="_blank" class="image-item">
                <img v-lazy="image.preview" />
              </a>
            </li>
          </ul>

          <div class="topic-actions">
            <!-- <a v-if="item.status === 0" class="action-item btn" @click="deleteSubmit(item.id)"
              >删除</a
            > -->
            <el-button
              v-if="item.status === 0"
              type="text"
              class="action-item btn"
              @click="deleteSubmit(item.id)"
              >删除</el-button
            >
            <!-- <a v-else class="action-item btn" @click="undeleteSubmit(item.id)">取消删除</a> -->
            <el-button v-else type="text" class="action-item btn" @click="undeleteSubmit(item.id)"
              >取消删除</el-button
            >

            <!-- <a v-if="!item.recommend" class="action-item btn" @click="recommend(item.id)">推荐</a> -->
            <el-button
              v-if="!item.recommend"
              type="text"
              class="action-item btn"
              @click="recommend(item.id)"
              >推荐</el-button
            >
            <!-- <a v-else class="action-item btn" @click="cancelRecommend(item.id)">取消推荐</a> -->
            <el-button v-else type="text" class="action-item btn" @click="cancelRecommend(item.id)"
              >取消推荐</el-button
            >

            <el-button
              v-if="!item.sticky"
              type="text"
              class="action-item btn"
              @click="sticky(item.id)"
              >置顶</el-button
            >
            <!-- <a v-else class="action-item btn" @click="cancelRecommend(item.id)">取消推荐</a> -->
            <el-button v-else type="text" class="action-item btn" @click="cancelSticky(item.id)"
              >取消置顶</el-button
            >
          </div>
        </div>
      </div>
    </div>

    <div class="pagebar">
      <el-pagination
        :page-sizes="[20, 50, 100, 300]"
        :current-page="page.page"
        :page-size="page.limit"
        :total="page.total"
        layout="total, sizes, prev, pager, next, jumper"
        @current-change="handlePageChange"
        @size-change="handleLimitChange"
      />
    </div>

    <el-dialog :visible.sync="addFormVisible" :close-on-click-modal="false" title="新增">
      <el-form ref="addForm" :model="addForm" :rules="addFormRules" label-width="80px">
        <el-form-item label="userId" prop="rule">
          <el-input v-model="addForm.userId" />
        </el-form-item>

        <el-form-item label="title" prop="rule">
          <el-input v-model="addForm.title" />
        </el-form-item>

        <el-form-item label="content" prop="rule">
          <el-input v-model="addForm.content" />
        </el-form-item>

        <el-form-item label="status" prop="rule">
          <el-input v-model="addForm.status" />
        </el-form-item>

        <el-form-item label="createTime" prop="rule">
          <el-input v-model="addForm.createTime" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native="addFormVisible = false"> 取消 </el-button>
        <el-button :loading="addLoading" type="primary" @click.native="addSubmit"> 提交 </el-button>
      </div>
    </el-dialog>

    <el-dialog :visible.sync="editFormVisible" :close-on-click-modal="false" title="编辑">
      <el-form ref="editForm" :model="editForm" :rules="editFormRules" label-width="80px">
        <el-input v-model="editForm.id" type="hidden" />

        <el-form-item label="forumId" prop="rule">
          <el-input v-model="editForm.forumId" />
        </el-form-item>

        <el-form-item label="userId" prop="rule">
          <el-input v-model="editForm.userId" />
        </el-form-item>

        <el-form-item label="title" prop="rule">
          <el-input v-model="editForm.title" />
        </el-form-item>

        <el-form-item label="content" prop="rule">
          <el-input v-model="editForm.content" />
        </el-form-item>

        <el-form-item label="status" prop="rule">
          <el-input v-model="editForm.status" />
        </el-form-item>

        <el-form-item label="createTime" prop="rule">
          <el-input v-model="editForm.createTime" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native="editFormVisible = false"> 取消 </el-button>
        <el-button :loading="editLoading" type="primary" @click.native="editSubmit">
          提交
        </el-button>
      </div>
    </el-dialog>
  </section>
</template>

<script>
import Avatar from "@/components/Avatar";
export default {
  components: { Avatar },
  data() {
    return {
      results: [],
      listLoading: false,
      page: {},
      filters: {
        // status: "0",
      },
      selectedRows: [],
      addForm: {
        forumId: "",
        userId: "",
        title: "",
        content: "",
        status: "",
        createTime: "",
      },
      addFormVisible: false,
      addFormRules: {},
      addLoading: false,

      editForm: {
        id: "",
        forumId: "",
        userId: "",
        title: "",
        content: "",
        status: "",
        createTime: "",
      },
      editFormVisible: false,
      editFormRules: {},
      editLoading: false,
    };
  },
  mounted() {
    this.list();
  },
  methods: {
    list() {
      const me = this;
      me.listLoading = true;
      const params = Object.assign(me.filters, {
        skipCount: this.getSkipCount(me.page.page, me.page.limit),
        maxResultCount: me.page.limit,
      });
      this.axios
        .post("/api/app/topic/paged", params)
        .then((data) => {
          me.results = data.items;
          me.page.total = data.totalCount;
        })
        .finally(() => {
          me.listLoading = false;
        });
    },
    handlePageChange(val) {
      this.page.page = val;
      this.list();
    },
    handleLimitChange(val) {
      this.page.limit = val;
      this.list();
    },
    handleAdd() {
      this.addForm = {
        name: "",
        description: "",
      };
      this.addFormVisible = true;
    },
    addSubmit() {
      const me = this;
      this.axios
        .form("/api/admin/topic/create", this.addForm)
        .then((data) => {
          me.$message({ message: "提交成功", type: "success" });
          me.addFormVisible = false;
          me.list();
        })
        .catch((rsp) => {
          me.$notify.error({ title: "错误", message: rsp.message });
        });
    },
    handleEdit(index, row) {
      const me = this;
      this.axios
        .get(`/api/admin/topic/${row.id}`)
        .then((data) => {
          me.editForm = Object.assign({}, data);
          me.editFormVisible = true;
        })
        .catch((rsp) => {
          me.$notify.error({ title: "错误", message: rsp.message });
        });
    },
    editSubmit() {
      const me = this;
      this.axios
        .form("/api/admin/topic/update", me.editForm)
        .then((data) => {
          me.list();
          me.editFormVisible = false;
        })
        .catch((rsp) => {
          me.$notify.error({ title: "错误", message: rsp.message });
        });
    },
    deleteSubmit(topicId) {
      const me = this;
      this.$confirm("是否确认删除该话题?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(function () {
          me.axios
            .post("/api/app/topic/" + topicId + "/set-delete?status=1")
            .then(function () {
              me.$message({ message: "删除成功", type: "success" });
              me.list();
            })
            .catch(function (err) {
              me.$notify.error({ title: "错误", message: err.message || err });
            });
        })
        .catch(function () {
          me.$message({
            type: "info",
            message: "已取消删除",
          });
        });
    },
    async undeleteSubmit(topicId) {
      try {
        await this.axios.post("/api/app/topic/" + topicId + "/set-delete?status=0");
        this.list();
        this.$message({ message: "取消删除成功", type: "success" });
      } catch (err) {
        this.$notify.error({ title: "错误", message: err.message || err });
      }
    },
    async recommend(id) {
      try {
        await this.axios.post("/api/app/topic/" + id + "/set-recommend?recommend=true");
        this.$message({ message: "推荐成功", type: "success" });
        this.list();
      } catch (e) {
        this.$notify.error({ title: "错误", message: e.message });
      }
    },
    async cancelRecommend(id) {
      try {
        await this.axios.post("/api/app/topic/" + id + "/set-recommend?recommend=false");
        this.$message({ message: "取消推荐成功", type: "success" });
        this.list();
      } catch (e) {
        this.$notify.error({ title: "错误", message: e.message });
      }
    },
    async sticky(id) {
      try {
        await this.axios.post("/api/app/topic/sticky/" + id + "?sticky=true");
        this.$message({ message: "置顶成功", type: "success" });
        this.list();
      } catch (e) {
        this.$notify.error({ title: "错误", message: e.message });
      }
    },
    async cancelSticky(id) {
      try {
        await this.axios.post("/api/app/topic/sticky/" + id + "?sticky=false");
        this.$message({ message: "取消置顶成功", type: "success" });
        this.list();
      } catch (e) {
        this.$notify.error({ title: "错误", message: e.message });
      }
    },
    handleSelectionChange(val) {
      this.selectedRows = val;
    },
  },
};
</script>

<style scoped lang="scss">
.topic-sticky-icon {
  color: red;
  border: 1px solid red;
  border-radius: 2px;
  font-size: 12px;
  font-weight: 700;
  padding: 0 5px;
  margin-left: 10px;
}

.topics {
  width: 100%;

  .topic-item:not(:last-child) {
    border-bottom: solid 1px rgba(140, 147, 157, 0.14);
  }

  .topic-item {
    width: 100%;
    padding: 10px;
    display: flex;
    flex: 1;

    .topic-main {
      width: 100%;
      margin-left: 10px;
      font-size: 14px;
      a {
        font-size: 14px;
      }

      .topic-header {
        .topic-nickname {
          font-size: 16px;
          font-weight: 600;
          color: #606266;
        }

        .topic-info {
          margin-left: 10px;
          float: right;
          cursor: pointer;
        }
      }

      .topic-title {
        margin-top: 6px;
        a {
          font-size: 16px;
          font-weight: 600;
          color: #000;
        }
      }

      .topic-metadata {
        margin-top: 6px;
        color: #8590a6;

        .topic-metadata-item {
          margin-right: 12px;
        }
      }

      .topic-summary {
        margin-top: 6px;
        color: #525252;
      }

      .topic-image-list {
        margin-top: 6px;
        li {
          cursor: pointer;
          border: 1px dashed #ddd;
          text-align: center;

          // 图片尺寸
          $image-size: 120px;

          display: inline-block;
          vertical-align: middle;
          width: $image-size;
          height: $image-size;
          line-height: $image-size;
          margin: 0 8px 8px 0;
          background-color: #e8e8e8;
          background-size: 32px 32px;
          background-position: 50%;
          background-repeat: no-repeat;
          overflow: hidden;
          position: relative;

          .image-item {
            display: block;
            width: $image-size;
            height: $image-size;
            overflow: hidden;
            transform-style: preserve-3d;

            & > img {
              width: 100%;
              height: 100%;
              object-fit: cover;
              transition: all 0.5s ease-out 0.1s;

              &:hover {
                transform: matrix(1.04, 0, 0, 1.04, 0, 0);
                backface-visibility: hidden;
              }
            }
          }
        }
      }

      .topic-actions {
        margin-top: 6px;
        text-align: right;

        .action-item {
          margin-left: 12px;
        }
      }
    }
  }
}
</style>
