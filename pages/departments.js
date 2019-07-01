var $url = 'pages/departments';

var data = {
  siteId: utils.getQueryString('siteId'),
  apiUrl: utils.getQueryString('apiUrl'),
  pageLoad: false,
  pageAlert: null,
  pageType: null,
  items: null,
  tableName: null,
  relatedIdentities: null
};

var methods = {
  getList: function () {
    var $this = this;

    $api.get($url, {
      params: {
        siteId: $this.siteId
      }
    }).then(function (response) {
      var res = response.data;

      $this.items = res.value;
      $this.tableName = res.tableName;
      $this.relatedIdentities = res.relatedIdentities;
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  btnEditClick: function (departmenteId) {
    var $this = this;
    utils.openLayer({
      title: '编辑部门',
      url: 'departmentsLayerAdd.html?siteId=' + $this.siteId + '&departmenteId=' + departmenteId + '&apiUrl=' + encodeURIComponent($this.apiUrl)
    });
  },

  btnAddClick: function () {
    var $this = this;
    utils.openLayer({
      title: '新增部门',
      url: 'departmentsLayerAdd.html?siteId=' + $this.siteId + '&apiUrl=' + encodeURIComponent($this.apiUrl)
    });
  },

  btnDeleteClick: function (departmentInfo) {
    var $this = this;

    utils.alertDelete({
      title: '删除部门',
      text: '此操作将删除部门 ' + departmentInfo.departmentName + '，确定吗？',
      callback: function () {
        utils.loading(true);
        $api.delete($url + '/' + departmentInfo.id + '?siteId=' + $this.siteId + '&apiUrl=' + encodeURIComponent($this.apiUrl)).then(function (response) {
          var res = response.data;
          $this.items = res.value;
        }).catch(function (error) {
          $this.pageAlert = utils.getPageAlert(error);
        }).then(function () {
          utils.loading(false);
        });
      }
    });
  }
};

new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.getList();
  }
});