(function (global, factory) {
	typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
	typeof define === 'function' && define.amd ? define(factory) :
	(global.VueStickyDirective = factory());
}(this, (function () { 'use strict';

var classCallCheck = function (instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
};

var createClass = function () {
  function defineProperties(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  return function (Constructor, protoProps, staticProps) {
    if (protoProps) defineProperties(Constructor.prototype, protoProps);
    if (staticProps) defineProperties(Constructor, staticProps);
    return Constructor;
  };
}();

var namespace = '@@vue-sticky-directive';
var events = ['resize', 'scroll', 'touchstart', 'touchmove', 'touchend', 'pageshow', 'load'];

var batchStyle = function batchStyle(el) {
  var style = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : {};
  var className = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : {};

  for (var k in style) {
    el.style[k] = style[k];
  }
  for (var _k in className) {
    if (className[_k] && !el.classList.contains(_k)) {
      el.classList.add(_k);
    } else if (!className[_k] && el.classList.contains(_k)) {
      el.classList.remove(_k);
    }
  }
};

var Sticky$1 = function () {
  function Sticky(el, vm) {
    classCallCheck(this, Sticky);

    this.el = el;
    this.vm = vm;
    this.unsubscribers = [];
    this.isPending = false;
    this.state = {
      isTopSticky: null,
      isBottomSticky: null,
      height: null,
      width: null,
      xOffset: null
    };

    this.lastState = {
      top: null,
      bottom: null,
      sticked: false
    };

    var offset = this.getAttribute('sticky-offset') || {};
    var side = this.getAttribute('sticky-side') || 'top';
    var zIndex = this.getAttribute('sticky-z-index') || '10';
    var onStick = this.getAttribute('on-stick') || null;

    this.options = {
      topOffset: Number(offset.top) || 0,
      bottomOffset: Number(offset.bottom) || 0,
      shouldTopSticky: side === 'top' || side === 'both',
      shouldBottomSticky: side === 'bottom' || side === 'both',
      zIndex: zIndex,
      onStick: onStick
    };
  }

  createClass(Sticky, [{
    key: 'doBind',
    value: function doBind() {
      var _this = this;

      if (this.unsubscribers.length > 0) {
        return;
      }
      var el = this.el,
          vm = this.vm;

      vm.$nextTick(function () {
        _this.placeholderEl = document.createElement('div');
        _this.containerEl = _this.getContainerEl();
        el.parentNode.insertBefore(_this.placeholderEl, el);
        events.forEach(function (event) {
          var fn = _this.update.bind(_this);
          _this.unsubscribers.push(function () {
            return window.removeEventListener(event, fn);
          });
          _this.unsubscribers.push(function () {
            return _this.containerEl.removeEventListener(event, fn);
          });
          window.addEventListener(event, fn, { passive: true });
          _this.containerEl.addEventListener(event, fn, { passive: true });
        });
      });
    }
  }, {
    key: 'doUnbind',
    value: function doUnbind() {
      this.unsubscribers.forEach(function (fn) {
        return fn();
      });
      this.unsubscribers = [];
      this.resetElement();
    }
  }, {
    key: 'update',
    value: function update() {
      var _this2 = this;

      if (!this.isPending) {
        requestAnimationFrame(function () {
          _this2.isPending = false;
          _this2.recomputeState();
          _this2.updateElements();
        });
        this.isPending = true;
      }
    }
  }, {
    key: 'isTopSticky',
    value: function isTopSticky() {
      if (!this.options.shouldTopSticky) return false;
      var fromTop = this.state.placeholderElRect.top;
      var fromBottom = this.state.containerElRect.bottom;

      var topBreakpoint = this.options.topOffset;
      var bottomBreakpoint = this.options.bottomOffset;

      return fromTop <= topBreakpoint && fromBottom >= bottomBreakpoint;
    }
  }, {
    key: 'isBottomSticky',
    value: function isBottomSticky() {
      if (!this.options.shouldBottomSticky) return false;
      var fromBottom = window.innerHeight - this.state.placeholderElRect.top - this.state.height;
      var fromTop = window.innerHeight - this.state.containerElRect.top;

      var topBreakpoint = this.options.topOffset;
      var bottomBreakpoint = this.options.bottomOffset;

      return fromBottom <= bottomBreakpoint && fromTop >= topBreakpoint;
    }
  }, {
    key: 'recomputeState',
    value: function recomputeState() {
      this.state = Object.assign({}, this.state, {
        height: this.getHeight(),
        width: this.getWidth(),
        xOffset: this.getXOffset(),
        placeholderElRect: this.getPlaceholderElRect(),
        containerElRect: this.getContainerElRect()
      });
      this.state.isTopSticky = this.isTopSticky();
      this.state.isBottomSticky = this.isBottomSticky();
    }
  }, {
    key: 'fireEvents',
    value: function fireEvents() {
      if (typeof this.options.onStick === 'function' && (this.lastState.top !== this.state.isTopSticky || this.lastState.bottom !== this.state.isBottomSticky || this.lastState.sticked !== (this.state.isTopSticky || this.state.isBottomSticky))) {
        this.lastState = {
          top: this.state.isTopSticky,
          bottom: this.state.isBottomSticky,
          sticked: this.state.isBottomSticky || this.state.isTopSticky
        };
        this.options.onStick(this.lastState);
      }
    }
  }, {
    key: 'updateElements',
    value: function updateElements() {
      var placeholderStyle = { paddingTop: 0 };
      var elStyle = {
        position: 'static',
        top: 'auto',
        bottom: 'auto',
        left: 'auto',
        width: 'auto',
        zIndex: this.options.zIndex
      };
      var placeholderClassName = { 'vue-sticky-placeholder': true };
      var elClassName = {
        'vue-sticky-el': true,
        'top-sticky': false,
        'bottom-sticky': false
      };

      if (this.state.isTopSticky) {
        elStyle.position = 'fixed';
        elStyle.top = this.options.topOffset + 'px';
        elStyle.left = this.state.xOffset + 'px';
        elStyle.width = this.state.width + 'px';
        var bottomLimit = this.state.containerElRect.bottom - this.state.height - this.options.bottomOffset - this.options.topOffset;
        if (bottomLimit < 0) {
          elStyle.top = bottomLimit + this.options.topOffset + 'px';
        }
        placeholderStyle.paddingTop = this.state.height + 'px';
        elClassName['top-sticky'] = true;
      } else if (this.state.isBottomSticky) {
        elStyle.position = 'fixed';
        elStyle.bottom = this.options.bottomOffset + 'px';
        elStyle.left = this.state.xOffset + 'px';
        elStyle.width = this.state.width + 'px';
        var topLimit = window.innerHeight - this.state.containerElRect.top - this.state.height - this.options.bottomOffset - this.options.topOffset;
        if (topLimit < 0) {
          elStyle.bottom = topLimit + this.options.bottomOffset + 'px';
        }
        placeholderStyle.paddingTop = this.state.height + 'px';
        elClassName['bottom-sticky'] = true;
      } else {
        placeholderStyle.paddingTop = 0;
      }

      batchStyle(this.el, elStyle, elClassName);
      batchStyle(this.placeholderEl, placeholderStyle, placeholderClassName);

      this.fireEvents();
    }
  }, {
    key: 'resetElement',
    value: function resetElement() {
      var _this3 = this;

      ['position', 'top', 'bottom', 'left', 'width', 'zIndex'].forEach(function (attr) {
        _this3.el.style.removeProperty(attr);
      });
      var parentNode = this.placeholderEl.parentNode;

      if (parentNode) {
        parentNode.removeChild(this.placeholderEl);
      }
    }
  }, {
    key: 'getContainerEl',
    value: function getContainerEl() {
      var node = this.el.parentNode;
      while (node && node.tagName !== 'HTML' && node.tagName !== 'BODY' && node.nodeType === 1) {
        if (node.hasAttribute('sticky-container')) {
          return node;
        }
        node = node.parentNode;
      }
      return this.el.parentNode;
    }
  }, {
    key: 'getXOffset',
    value: function getXOffset() {
      return this.placeholderEl.getBoundingClientRect().left;
    }
  }, {
    key: 'getWidth',
    value: function getWidth() {
      return this.placeholderEl.getBoundingClientRect().width;
    }
  }, {
    key: 'getHeight',
    value: function getHeight() {
      return this.el.getBoundingClientRect().height;
    }
  }, {
    key: 'getPlaceholderElRect',
    value: function getPlaceholderElRect() {
      return this.placeholderEl.getBoundingClientRect();
    }
  }, {
    key: 'getContainerElRect',
    value: function getContainerElRect() {
      return this.containerEl.getBoundingClientRect();
    }
  }, {
    key: 'getAttribute',
    value: function getAttribute(name) {
      var expr = this.el.getAttribute(name);
      if (expr) {
        return this.vm[expr] || expr;
      } else {
        return undefined;
      }
    }
  }]);
  return Sticky;
}();

var Sticky$2 = {
  inserted: function inserted(el, bind, vnode) {
    if (typeof bind.value === 'undefined' || bind.value) {
      el[namespace] = new Sticky$1(el, vnode.context);
      el[namespace].doBind();
    }
  },
  unbind: function unbind(el, bind, vnode) {
    if (el[namespace]) {
      el[namespace].doUnbind();
      el[namespace] = undefined;
    }
  },
  componentUpdated: function componentUpdated(el, bind, vnode) {
    if (typeof bind.value === 'undefined' || bind.value) {
      if (!el[namespace]) {
        el[namespace] = new Sticky$1(el, vnode.context);
      }
      el[namespace].doBind();
    } else {
      if (el[namespace]) {
        el[namespace].doUnbind();
      }
    }
  }
};

var install = function install(Vue) {
  Vue.directive('Sticky', Sticky$2);
};

if (window.Vue) {
  Vue.use(install);
}

Sticky$2.install = install;

return Sticky$2;

})));
