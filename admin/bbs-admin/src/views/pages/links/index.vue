<template>
  <section class="page-container">
    <div class="toolbar">
      <el-form :inline="true" :model="filters">
        <el-form-item>
          <el-input v-model="filters.title" clearable placeholder="标题" />
        </el-form-item>
        <el-form-item>
          <el-input v-model="filters.url" clearable placeholder="链接" />
        </el-form-item>
        <!-- <el-form-item>
          <el-select v-model="filters.status" clearable placeholder="请选择状态" @change="list">
            <el-option label="正常" value="0" />
            <el-option label="删除" value="1" />
          </el-select>
        </el-form-item> -->
        <el-form-item>
          <el-button type="primary" @click="list"> 查询 </el-button>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </div>

    <el-table
      v-loading="listLoading"
      :data="results"
      highlight-current-row
      stripe
      style="width: 100%"
    >
      <el-table-column prop="id" label="编号" width="100" />
      <el-table-column prop="title" label="标题" width="180" />
      <el-table-column prop="url" label="链接" width="250">
        <template slot-scope="scope">
          <a :href="scope.row.url" target="_blank">{{ scope.row.url }}</a>
        </template>
      </el-table-column>
      <el-table-column prop="logo" label="图标" width="100">
        <template slot-scope="scope">
          <img v-if="scope.row.logo" :src="scope.row.logo" class="link-logo" />
          <img v-else src="@/assets/images/net.png" class="link-logo" />
        </template>
      </el-table-column>
      <el-table-column prop="summary" label="描述" />
      <el-table-column prop="status" label="状态" width="120">
        <template slot-scope="scope">
          {{ scope.row.status === 0 ? "正常" : "禁用" }}
        </template>
      </el-table-column>
      <el-table-column prop="createTime" label="创建时间" width="180">
        <template slot-scope="scope">
          {{ scope.row.createTime | formatDate }}
        </template>
      </el-table-column>
      <el-table-column label="操作" width="150">
        <template slot-scope="scope">
          <el-button size="small" @click="handleEdit(scope.$index, scope.row)"> 编辑 </el-button>
          <el-button size="small" @click="handleDel(scope.$index, scope.row)"> 删除 </el-button>
        </template>
      </el-table-column>
    </el-table>

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
      <el-form ref="addForm" :model="addForm" label-width="80px">
        <el-form-item label="链接">
          <el-input v-model="addForm.url" style="width: 80%" />&nbsp;
          <el-button type="primary" :loading="detectLoading" @click="detect"> Detect </el-button>
        </el-form-item>
        <el-form-item label="标题">
          <el-input v-model="addForm.title" />
        </el-form-item>
        <el-form-item label="描述">
          <el-input v-model="addForm.summary" :autosize="{ minRows: 2, maxRows: 4 }" />
        </el-form-item>
        <el-form-item label="图标">
          <!-- <el-input v-model="addForm.logo" /> -->
          <upload v-model="addForm.logo" />
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
        <el-form-item label="链接">
          <el-input v-model="editForm.url" />
        </el-form-item>
        <el-form-item label="标题">
          <el-input v-model="editForm.title" />
        </el-form-item>
        <el-form-item label="描述">
          <el-input v-model="editForm.summary" :autosize="{ minRows: 2, maxRows: 4 }" />
        </el-form-item>
        <el-form-item label="Logo">
          <!-- <el-input v-model="editForm.logo" /> -->
          <upload v-model="editForm.logo" />
        </el-form-item>
        <el-form-item label="状态" prop="status">
          <el-select v-model="editForm.status" placeholder="请选择">
            <el-option :key="0" :value="0" label="正常" />
            <el-option :key="1" :value="1" label="禁用" />
          </el-select>
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
import Upload from "@/components/Upload";
export default {
  components: { Upload },
  data() {
    return {
      results: [],
      listLoading: false,
      page: {},
      filters: {},

      addForm: {},
      addFormVisible: false,
      addFormRules: {},
      addLoading: false,
      detectLoading: false,

      editForm: {},
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
        .post("/api/app/friendly-link/list", params)
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
      this.addForm = {};
      this.addFormVisible = true;
    },
    addSubmit() {
      const me = this;
      this.axios
        .post("/api/app/friendly-link", this.addForm)
        .then((data) => {
          me.$message({ message: "提交成功", type: "success" });
          me.addFormVisible = false;
          me.list();
        })
        .catch((rsp) => {
          me.$notify.error({ title: "错误", message: rsp.message });
        });
    },
    async detect() {
      if (!this.addForm.url) {
        return;
      }
      try {
        const flag = await this.$confirm("确定采集吗，采集之后将覆盖现有内容?", "提示", {
          type: "warning",
        });
        if (flag) {
          this.detectLoading = true;
          const data = await this.axios.post("/api/app/friendly-link/detect", {
            url: this.addForm.url,
          });
          if (data && data.title) {
            this.$set(this.addForm, "title", data.title);
            this.$set(this.addForm, "summary", data.description);
          }
        }
      } catch (e) {
        this.$notify.error({ title: "错误", message: e.message || e });
      } finally {
        this.detectLoading = false;
      }
    },
    handleEdit(index, row) {
      const me = this;
      this.axios
        .get(`/api/app/friendly-link/${row.id}`)
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
        .put("/api/app/friendly-link/" + me.editForm.id, me.editForm)
        .then((data) => {
          me.list();
          me.editFormVisible = false;
        })
        .catch((rsp) => {
          me.$notify.error({ title: "错误", message: rsp.message });
        });
    },
    handleDel(index, row) {
      const me = this;
      this.$confirm("此操作将永久删除该数据, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          this.axios
            .delete("/api/app/friendly-link/" + row.id)
            .then((data) => {
              me.$message({ message: "删除成功", type: "success" });
              me.list();
            })
            .catch((rsp) => {
              me.$notify.error({ title: "错误", message: rsp.message });
            });
        })
        .catch(() => {});
    },
  },
};
</script>

<style scoped>
.link-logo {
  max-width: 50px;
  max-height: 50px;
}
</style>
