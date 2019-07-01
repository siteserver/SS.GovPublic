VeeValidate.Validator.localize('zh_CN');
Vue.use(VeeValidate);
VeeValidate.Validator.localize({
  zh_CN: {
    messages: {
      required: function (name) {
        return name + '不能为空';
      }
    }
  }
});
VeeValidate.Validator.extend('mobile', {
  getMessage: function () {
    return ' 请输入正确的手机号码';
  },
  validate: function (value, args) {
    return (
      value.length == 11 &&
      /^((13|14|15|16|17|18|19)[0-9]{1}\d{8})$/.test(value)
    );
  }
});

var getQueryString = function (name) {
  var result = location.search.match(
    new RegExp('[?&]' + name + '=([^&]+)', 'i')
  );
  if (!result || result.length < 1) {
    return '';
  }
  return decodeURIComponent(result[1]);
};

var getPageAlert = function (error) {
  var message = error.message;
  if (error.response && error.response.data) {
    if (error.response.data.exceptionMessage) {
      message = error.response.data.exceptionMessage;
    } else if (error.response.data.message) {
      message = error.response.data.message;
    }
  }

  return {
    type: "danger",
    html: message
  };
};

var $api = axios.create({
  baseURL: (getQueryString('apiUrl') || $apiUrl) + '/SS.Application/',
  withCredentials: true
});

var data = {
  siteId: getQueryString('siteId') || $siteId,
  apiUrl: getQueryString('apiUrl') || $apiUrl,
  pageConfig: null,
  pageLoad: false,
  pageType: 'form',
  pageAlert: null,
  dataInfo: null,
  departmentInfoList: null,
  settings: null,
  provideType: [],
  obtainType: []
};

var methods = {
  load: function () {
    var $this = this;

    $api.get('?siteId=' + this.siteId).then(function (response) {
      var res = response.data;

      $this.dataInfo = res.value;
      $this.departmentInfoList = res.departmentInfoList;
      $this.settings = res.settings;
      $this.provideType = [];
      $this.obtainType = [];
      if ($this.settings.isClosed) {
        $this.pageType = 'error';
        $this.errorMessage = '依申请公开已暂时关闭！';
      }
    }).catch(function (error) {
      $this.pageAlert = getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  submit: function () {
    var $this = this;

    $this.pageLoad = false;
    $api.post('?siteId=' + this.siteId, _.assign({}, this.dataInfo, {
      provideType: $this.provideType,
      obtainType: $this.obtainType
    })).then(function (response) {
      var res = response.data;
      $this.dataInfo = res.value;
      $this.pageType = 'success';
    }).catch(function (error) {
      $this.pageAlert = getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    this.pageAlert = null;

    this.$validator.validate().then(function (result) {
      if (result) {
        $this.submit();
      }
    });
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.load();
  }
});