if (window.VeeValidate) {
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
}

var PER_DAY = 1000 * 60 * 60 * 24;

var utils = {
  getStateText: function (state, clearTags) {
    if (state == 'New') return clearTags ? '新申请，待受理' : '<span class="text-success">新申请，待受理</span>';
    else if (state == 'Denied') return clearTags ? '拒绝受理' : '<span class="text-danger">拒绝受理</span>';
    else if (state == 'Accepted') return clearTags ? '已受理，待办理' : '<span class="text-success">已受理，待办理</span>';
    else if (state == 'Redo') return clearTags ? '要求返工' : '<span class="text-warning">要求返工</span>';
    else if (state == 'Replied') return clearTags ? '已办理，待审核' : '<span class="text-success">已办理，待审核</span>';
    else if (state == 'Checked') return clearTags ? '已审核，处理完毕' : '<span class="text-primary">已审核，处理完毕</span>';
    else return '';
  },

  dateDiffInDays: function (a, b) {
    // Discard the time and time-zone information.
    const utc1 = new Date(a.getFullYear(), a.getMonth(), a.getDate());
    const utc2 = new Date(b.getFullYear(), b.getMonth(), b.getDate());

    return Math.floor((utc1 - utc2) / PER_DAY);
  },

  getDays: function (content, settings) {
    if (!content || !settings || content.state == 'Checked' | content.state == 'Denied') return '';
    var diffDays = utils.dateDiffInDays(new Date(), new Date(content.addDate));
    var badge = '';
    if (settings.daysDeadline > 0 && diffDays >= settings.daysDeadline) {
      badge = '<br /><span class="badge badge-danger">已超过办理期限 <strong>' + (diffDays - settings.daysDeadline) + '</strong> 天，请尽快办理</span></div>';
    } else if (settings.daysWarning > 0 && diffDays >= settings.daysWarning) {
      badge = '<br /><span class="badge badge-warning">已超过预警期限 <strong>' + (diffDays - settings.daysWarning) + '</strong> 天，请尽快办理</span></div>';
    }

    return badge;
  },

  getQueryString: function (name) {
    var result = location.search.match(
      new RegExp('[?&]' + name + '=([^&]+)', 'i')
    );
    if (!result || result.length < 1) {
      return '';
    }
    return decodeURIComponent(result[1]);
  },

  getPageAlert: function (error) {
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
  },

  loading: function (isLoading) {
    if (isLoading) {
      return layer.load(1, {
        shade: [0.2, '#000']
      });
    } else {
      layer.close(layer.index);
    }
  },

  closeLayer: function () {
    parent.layer.closeAll();
    return false;
  },

  openLayer: function (config) {
    if (!config || !config.url) return false;

    if (!config.width) {
      config.width = $(window).width() - 50;
    }
    if (!config.height) {
      config.height = $(window).height() - 50;
    }

    if (config.full) {
      config.width = $(window).width() - 50;
      config.height = $(window).height() - 50;
    }

    layer.open({
      type: 2,
      btn: null,
      title: config.title,
      area: [config.width + 'px', config.height + 'px'],
      maxmin: true,
      resize: true,
      shadeClose: true,
      content: config.url
    });

    return false;
  },

  alertError: function (err) {
    swal({
      title: '系统错误！',
      text: '请联系管理员协助解决',
      type: 'error',
      showConfirmButton: false,
      allowOutsideClick: false,
      allowEscapeKey: false,
      confirmButtonClass: 'btn btn-primary',
      cancelButtonClass: 'btn btn-default ml-3',
      buttonsStyling: false,
    });
  },

  alertDelete: function (config) {
    if (!config) return false;

    swal({
      title: config.title,
      text: config.text,
      type: 'question',
      confirmButtonText: '确认删除',
      confirmButtonClass: 'btn btn-danger',
      showCancelButton: true,
      cancelButtonText: '取 消',
      cancelButtonClass: 'btn btn-default ml-3',
      buttonsStyling: false,
    }).then(function (result) {
      if (result.value) {
        config.callback();
      }
    });

    return false;
  }
};

var $api = axios.create({
  baseURL: utils.getQueryString('apiUrl') + '/SS.GovPublic/',
  withCredentials: true
});