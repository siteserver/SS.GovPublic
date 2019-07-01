var getQueryString = function (name) {
  var result = location.search.match(
    new RegExp('[?&]' + name + '=([^&]+)', 'i')
  );
  if (!result || result.length < 1) {
    return '';
  }
  return decodeURIComponent(result[1]);
};

var getStateText = function (isCompleted, state) {
  if (!isCompleted) return '<span class="text-warning">申请正在处理中，请等待处理结果</span>';
  else if (state == 'Denied') return '<span class="text-danger">拒绝受理</span>';
  else if (state == 'Checked') return '<span class="text-primary">处理完毕</span>';
  else return '';
};

var $api = axios.create({
  withCredentials: true
});

var $apiUrl = (getQueryString('apiUrl') || $apiUrl) + '/SS.Application/actions/query?siteId=' + (getQueryString('siteId') || $siteId);

var data = {
  pageConfig: null,
  pageType: 'form',
  isOrganization: false,
  civicName: null,
  orgName: null,
  queryCode: null,
  isSubmit: false,
  isNoResult: false,
  dataInfo: null,
  fileInfoList: null,
};

var methods = {
  btnSubmitClick: function () {
    var $this = this;
    this.isSubmit = true;
    this.isNoResult = false;
    if (this.isOrganization) {
      if (!this.orgName) return;
    } else {
      if (!this.civicName) return;
    }
    if (!this.queryCode) return;

    $this.pageType = 'loading';
    $api.post($apiUrl, {
      isOrganization: $this.isOrganization,
      civicName: $this.civicName,
      orgName: $this.orgName,
      queryCode: $this.queryCode
    }).then(function (response) {
      var res = response.data;
      $this.dataInfo = res.value;
      $this.fileInfoList = res.fileInfoList;
      $this.pageType = 'success';
    }).catch(function (error) {
      $this.pageType = 'form';
      $this.isNoResult = true;
    });
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods
});