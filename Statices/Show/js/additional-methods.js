jQuery.validator.addMethod("byteRangeLength",
            function (value, element, param) {
                var length = value.replace(/[\n]/ig, '').length;
                for (var i = 0; i < value.replace(/[\n]/ig, '').length; i++) {
                    if (value.replace(/[\n]/ig, '').charCodeAt(i) > 127) {
                        length++;
                    }
                }
            return this.optional(element) || (length >= param[0] && length <= param[1]);},
            $.validator.format("请确保输入的值在{0}-{1}个字节之间(一个中文字算2个字节)")
        );
jQuery.validator.addMethod("pattern", function (value, element, param) {
    if (this.optional(element)) {
        return true;
    }
    if (typeof param === "string") {
        param = new RegExp("^(?:" + param + ")$");
    }
    return param.test(value);
}, "格式错误.");