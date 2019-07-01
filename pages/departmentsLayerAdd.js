var $url = '/pages/departmentsLayerAdd';

var data = {
  siteId: utils.getQueryString('siteId'),
  apiUrl: utils.getQueryString('apiUrl'),
  departmenteId: utils.getQueryString('departmenteId'),
  pageLoad: false,
  pageAlert: null,
  departmentInfo: null,
  allUserNames: null,
  userNames: []
};

var methods = {
  load: function () {
    var $this = this;

    if (this.departmenteId) {
      $api.get($url + '/' + this.departmenteId + '?siteId=' + this.siteId).then(function (response) {
        var res = response.data;
        $this.departmentInfo = res.value;
        $this.allUserNames = res.allUserNames;
        $this.userNames = $this.departmentInfo.userNames.split(',');
      }).catch(function (error) {
        $this.pageAlert = utils.getPageAlert(error);
      }).then(function () {
        $this.pageLoad = true;
      });
    } else {
      $api.get($url + '?siteId=' + this.siteId).then(function (response) {
        var res = response.data;
        $this.departmentInfo = res.value;
        $this.allUserNames = res.allUserNames;
        $this.userNames = $this.departmentInfo.userNames.split(',');
      }).catch(function (error) {
        $this.pageAlert = utils.getPageAlert(error);
      }).then(function () {
        $this.pageLoad = true;
      });
    }
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$validator.validate().then(function (result) {
      if (result) {
        utils.loading(true);

        if ($this.departmenteId) {
          $api.put($url + '/' + $this.departmenteId + '?siteId=' + $this.siteId, {
            departmentName: $this.departmentInfo.departmentName,
            departmentCode: $this.departmentInfo.departmentCode,
            userNames: $this.userNames.join(','),
            taxis: $this.departmentInfo.taxis
          }).then(function (response) {
            parent.location.reload(true);
          }).catch(function (error) {
            $this.pageAlert = utils.getPageAlert(error);
          }).then(function () {
            utils.loading(false);
          });
        } else {
          $api.post($url + '?siteId=' + $this.siteId, {
            departmentName: $this.departmentInfo.departmentName,
            departmentCode: $this.departmentInfo.departmentCode,
            userNames: $this.userNames.join(','),
            taxis: $this.departmentInfo.taxis
          }).then(function (response) {
            parent.location.reload(true);
          }).catch(function (error) {
            $this.pageAlert = utils.getPageAlert(error);
          }).then(function () {
            utils.loading(false);
          });
        }
      }
    });
  }
};

new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.load();
  }
});