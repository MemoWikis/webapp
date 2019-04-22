
    /*!
    * tiptap-extensions v1.6.0
    * (c) 2019 Scrumpy UG (limited liability)
    * @license MIT
    */
  
(function (global, factory) {
  typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('tiptap'), require('tiptap-commands'), require('prosemirror-view'), require('prosemirror-utils'), require('lowlight/lib/core'), require('prosemirror-state'), require('prosemirror-tables'), require('tiptap-utils'), require('prosemirror-history')) :
  typeof define === 'function' && define.amd ? define(['exports', 'tiptap', 'tiptap-commands', 'prosemirror-view', 'prosemirror-utils', 'lowlight/lib/core', 'prosemirror-state', 'prosemirror-tables', 'tiptap-utils', 'prosemirror-history'], factory) :
  factory(global.tiptap = {},global.tiptap,global.tiptapCommands,global.prosemirrorView,global.prosemirrorUtils,global.low,global.prosemirrorState,global.prosemirrorTables,global.tiptapUtils,global.prosemirrorHistory);
}(typeof self !== 'undefined' ? self : this, function (exports,tiptap,tiptapCommands,prosemirrorView,prosemirrorUtils,low,prosemirrorState,prosemirrorTables,tiptapUtils,prosemirrorHistory) { 'use strict';

  low = low && low.hasOwnProperty('default') ? low['default'] : low;

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties(Constructor, staticProps);
    return Constructor;
  }

  function _defineProperty(obj, key, value) {
    if (key in obj) {
      Object.defineProperty(obj, key, {
        value: value,
        enumerable: true,
        configurable: true,
        writable: true
      });
    } else {
      obj[key] = value;
    }

    return obj;
  }

  function _objectSpread(target) {
    for (var i = 1; i < arguments.length; i++) {
      var source = arguments[i] != null ? arguments[i] : {};
      var ownKeys = Object.keys(source);

      if (typeof Object.getOwnPropertySymbols === 'function') {
        ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) {
          return Object.getOwnPropertyDescriptor(source, sym).enumerable;
        }));
      }

      ownKeys.forEach(function (key) {
        _defineProperty(target, key, source[key]);
      });
    }

    return target;
  }

  function _inherits(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf(subClass, superClass);
  }

  function _getPrototypeOf(o) {
    _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf(o);
  }

  function _setPrototypeOf(o, p) {
    _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf(o, p);
  }

  function _assertThisInitialized(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _possibleConstructorReturn(self, call) {
    if (call && (typeof call === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized(self);
  }

  function _slicedToArray(arr, i) {
    return _arrayWithHoles(arr) || _iterableToArrayLimit(arr, i) || _nonIterableRest();
  }

  function _toConsumableArray(arr) {
    return _arrayWithoutHoles(arr) || _iterableToArray(arr) || _nonIterableSpread();
  }

  function _arrayWithoutHoles(arr) {
    if (Array.isArray(arr)) {
      for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) arr2[i] = arr[i];

      return arr2;
    }
  }

  function _arrayWithHoles(arr) {
    if (Array.isArray(arr)) return arr;
  }

  function _iterableToArray(iter) {
    if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter);
  }

  function _iterableToArrayLimit(arr, i) {
    var _arr = [];
    var _n = true;
    var _d = false;
    var _e = undefined;

    try {
      for (var _i = arr[Symbol.iterator](), _s; !(_n = (_s = _i.next()).done); _n = true) {
        _arr.push(_s.value);

        if (i && _arr.length === i) break;
      }
    } catch (err) {
      _d = true;
      _e = err;
    } finally {
      try {
        if (!_n && _i["return"] != null) _i["return"]();
      } finally {
        if (_d) throw _e;
      }
    }

    return _arr;
  }

  function _nonIterableSpread() {
    throw new TypeError("Invalid attempt to spread non-iterable instance");
  }

  function _nonIterableRest() {
    throw new TypeError("Invalid attempt to destructure non-iterable instance");
  }

  var Blockquote =
  /*#__PURE__*/
  function (_Node) {
    _inherits(Blockquote, _Node);

    function Blockquote() {
      _classCallCheck(this, Blockquote);

      return _possibleConstructorReturn(this, _getPrototypeOf(Blockquote).apply(this, arguments));
    }

    _createClass(Blockquote, [{
      key: "commands",
      value: function commands(_ref) {
        var type = _ref.type,
            schema = _ref.schema;
        return function () {
          return tiptapCommands.toggleWrap(type, schema.nodes.paragraph);
        };
      }
    }, {
      key: "keys",
      value: function keys(_ref2) {
        var type = _ref2.type;
        return {
          'Ctrl->': tiptapCommands.toggleWrap(type)
        };
      }
    }, {
      key: "inputRules",
      value: function inputRules(_ref3) {
        var type = _ref3.type;
        return [tiptapCommands.wrappingInputRule(/^\s*>\s$/, type)];
      }
    }, {
      key: "name",
      get: function get() {
        return 'blockquote';
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          content: 'block*',
          group: 'block',
          defining: true,
          draggable: false,
          parseDOM: [{
            tag: 'blockquote'
          }],
          toDOM: function toDOM() {
            return ['blockquote', 0];
          }
        };
      }
    }]);

    return Blockquote;
  }(tiptap.Node);

  var Bullet =
  /*#__PURE__*/
  function (_Node) {
    _inherits(Bullet, _Node);

    function Bullet() {
      _classCallCheck(this, Bullet);

      return _possibleConstructorReturn(this, _getPrototypeOf(Bullet).apply(this, arguments));
    }

    _createClass(Bullet, [{
      key: "commands",
      value: function commands(_ref) {
        var type = _ref.type,
            schema = _ref.schema;
        return function () {
          return tiptapCommands.toggleList(type, schema.nodes.list_item);
        };
      }
    }, {
      key: "keys",
      value: function keys(_ref2) {
        var type = _ref2.type,
            schema = _ref2.schema;
        return {
          'Shift-Ctrl-8': tiptapCommands.toggleList(type, schema.nodes.list_item)
        };
      }
    }, {
      key: "inputRules",
      value: function inputRules(_ref3) {
        var type = _ref3.type;
        return [tiptapCommands.wrappingInputRule(/^\s*([-+*])\s$/, type)];
      }
    }, {
      key: "name",
      get: function get() {
        return 'bullet_list';
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          content: 'list_item+',
          group: 'block',
          parseDOM: [{
            tag: 'ul'
          }],
          toDOM: function toDOM() {
            return ['ul', 0];
          }
        };
      }
    }]);

    return Bullet;
  }(tiptap.Node);

  var CodeBlock =
  /*#__PURE__*/
  function (_Node) {
    _inherits(CodeBlock, _Node);

    function CodeBlock() {
      _classCallCheck(this, CodeBlock);

      return _possibleConstructorReturn(this, _getPrototypeOf(CodeBlock).apply(this, arguments));
    }

    _createClass(CodeBlock, [{
      key: "commands",
      value: function commands(_ref) {
        var type = _ref.type,
            schema = _ref.schema;
        return function () {
          return tiptapCommands.toggleBlockType(type, schema.nodes.paragraph);
        };
      }
    }, {
      key: "keys",
      value: function keys(_ref2) {
        var type = _ref2.type;
        return {
          'Shift-Ctrl-\\': tiptapCommands.setBlockType(type)
        };
      }
    }, {
      key: "inputRules",
      value: function inputRules(_ref3) {
        var type = _ref3.type;
        return [tiptapCommands.textblockTypeInputRule(/^```$/, type)];
      }
    }, {
      key: "name",
      get: function get() {
        return 'code_block';
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          content: 'text*',
          marks: '',
          group: 'block',
          code: true,
          defining: true,
          draggable: false,
          parseDOM: [{
            tag: 'pre',
            preserveWhitespace: 'full'
          }],
          toDOM: function toDOM() {
            return ['pre', ['code', 0]];
          }
        };
      }
    }]);

    return CodeBlock;
  }(tiptap.Node);

  function getDecorations(doc) {
    var decorations = [];
    var blocks = prosemirrorUtils.findBlockNodes(doc).filter(function (item) {
      return item.node.type.name === 'code_block';
    });

    var flatten = function flatten(list) {
      return list.reduce(function (a, b) {
        return a.concat(Array.isArray(b) ? flatten(b) : b);
      }, []);
    };

    function parseNodes(nodes) {
      var className = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : [];
      return nodes.map(function (node) {
        var classes = _toConsumableArray(className).concat(_toConsumableArray(node.properties ? node.properties.className : []));

        if (node.children) {
          return parseNodes(node.children, classes);
        }

        return {
          text: node.value,
          classes: classes
        };
      });
    }

    blocks.forEach(function (block) {
      var startPos = block.pos + 1;
      var nodes = low.highlightAuto(block.node.textContent).value;
      flatten(parseNodes(nodes)).map(function (node) {
        var from = startPos;
        var to = from + node.text.length;
        startPos = to;
        return _objectSpread({}, node, {
          from: from,
          to: to
        });
      }).forEach(function (node) {
        var decoration = prosemirrorView.Decoration.inline(node.from, node.to, {
          class: node.classes.join(' ')
        });
        decorations.push(decoration);
      });
    });
    return prosemirrorView.DecorationSet.create(doc, decorations);
  }

  var CodeBlockHighlight =
  /*#__PURE__*/
  function (_Node) {
    _inherits(CodeBlockHighlight, _Node);

    function CodeBlockHighlight() {
      var _this;

      var options = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};

      _classCallCheck(this, CodeBlockHighlight);

      _this = _possibleConstructorReturn(this, _getPrototypeOf(CodeBlockHighlight).call(this, options));

      try {
        Object.entries(_this.options.languages).forEach(function (_ref) {
          var _ref2 = _slicedToArray(_ref, 2),
              name = _ref2[0],
              mapping = _ref2[1];

          low.registerLanguage(name, mapping);
        });
      } catch (err) {
        throw new Error('Invalid syntax highlight definitions: define at least one highlight.js language mapping');
      }

      return _this;
    }

    _createClass(CodeBlockHighlight, [{
      key: "commands",
      value: function commands(_ref3) {
        var type = _ref3.type,
            schema = _ref3.schema;
        return function () {
          return tiptapCommands.toggleBlockType(type, schema.nodes.paragraph);
        };
      }
    }, {
      key: "keys",
      value: function keys(_ref4) {
        var type = _ref4.type;
        return {
          'Shift-Ctrl-\\': tiptapCommands.setBlockType(type)
        };
      }
    }, {
      key: "inputRules",
      value: function inputRules(_ref5) {
        var type = _ref5.type;
        return [tiptapCommands.textblockTypeInputRule(/^```$/, type)];
      }
    }, {
      key: "defaultOptions",
      get: function get() {
        return {
          languages: {}
        };
      }
    }, {
      key: "name",
      get: function get() {
        return 'code_block';
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          content: 'text*',
          marks: '',
          group: 'block',
          code: true,
          defining: true,
          draggable: false,
          parseDOM: [{
            tag: 'pre',
            preserveWhitespace: 'full'
          }],
          toDOM: function toDOM() {
            return ['pre', ['code', 0]];
          }
        };
      }
    }, {
      key: "plugins",
      get: function get() {
        return [new tiptap.Plugin({
          state: {
            init: function init(_, _ref6) {
              var doc = _ref6.doc;
              return getDecorations(doc);
            },
            apply: function apply(tr, set) {
              // TODO: find way to cache decorations
              // see: https://discuss.prosemirror.net/t/how-to-update-multiple-inline-decorations-on-node-change/1493
              if (tr.docChanged) {
                return getDecorations(tr.doc);
              }

              return set.map(tr.mapping, tr.doc);
            }
          },
          props: {
            decorations: function decorations(state) {
              return this.getState(state);
            }
          }
        })];
      }
    }]);

    return CodeBlockHighlight;
  }(tiptap.Node);

  var HardBreak =
  /*#__PURE__*/
  function (_Node) {
    _inherits(HardBreak, _Node);

    function HardBreak() {
      _classCallCheck(this, HardBreak);

      return _possibleConstructorReturn(this, _getPrototypeOf(HardBreak).apply(this, arguments));
    }

    _createClass(HardBreak, [{
      key: "keys",
      value: function keys(_ref) {
        var type = _ref.type;
        var command = tiptapCommands.chainCommands(tiptapCommands.exitCode, function (state, dispatch) {
          dispatch(state.tr.replaceSelectionWith(type.create()).scrollIntoView());
          return true;
        });
        return {
          'Mod-Enter': command,
          'Shift-Enter': command
        };
      }
    }, {
      key: "name",
      get: function get() {
        return 'hard_break';
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          inline: true,
          group: 'inline',
          selectable: false,
          parseDOM: [{
            tag: 'br'
          }],
          toDOM: function toDOM() {
            return ['br'];
          }
        };
      }
    }]);

    return HardBreak;
  }(tiptap.Node);

  var Heading =
  /*#__PURE__*/
  function (_Node) {
    _inherits(Heading, _Node);

    function Heading() {
      _classCallCheck(this, Heading);

      return _possibleConstructorReturn(this, _getPrototypeOf(Heading).apply(this, arguments));
    }

    _createClass(Heading, [{
      key: "commands",
      value: function commands(_ref) {
        var type = _ref.type,
            schema = _ref.schema;
        return function (attrs) {
          return tiptapCommands.toggleBlockType(type, schema.nodes.paragraph, attrs);
        };
      }
    }, {
      key: "keys",
      value: function keys(_ref2) {
        var type = _ref2.type;
        return this.options.levels.reduce(function (items, level) {
          return _objectSpread({}, items, _defineProperty({}, "Shift-Ctrl-".concat(level), tiptapCommands.setBlockType(type, {
            level: level
          })));
        }, {});
      }
    }, {
      key: "inputRules",
      value: function inputRules(_ref3) {
        var type = _ref3.type;
        return this.options.levels.map(function (level) {
          return tiptapCommands.textblockTypeInputRule(new RegExp("^(#{1,".concat(level, "})\\s$")), type, function () {
            return {
              level: level
            };
          });
        });
      }
    }, {
      key: "name",
      get: function get() {
        return 'heading';
      }
    }, {
      key: "defaultOptions",
      get: function get() {
        return {
          levels: [1, 2, 3, 4, 5, 6]
        };
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          attrs: {
            level: {
              default: 1
            }
          },
          content: 'inline*',
          group: 'block',
          defining: true,
          draggable: false,
          parseDOM: this.options.levels.map(function (level) {
            return {
              tag: "h".concat(level),
              attrs: {
                level: level
              }
            };
          }),
          toDOM: function toDOM(node) {
            return ["h".concat(node.attrs.level), 0];
          }
        };
      }
    }]);

    return Heading;
  }(tiptap.Node);

  var Image =
  /*#__PURE__*/
  function (_Node) {
    _inherits(Image, _Node);

    function Image() {
      _classCallCheck(this, Image);

      return _possibleConstructorReturn(this, _getPrototypeOf(Image).apply(this, arguments));
    }

    _createClass(Image, [{
      key: "commands",
      value: function commands(_ref) {
        var type = _ref.type;
        return function (attrs) {
          return function (state, dispatch) {
            var selection = state.selection;
            var position = selection.$cursor ? selection.$cursor.pos : selection.$to.pos;
            var node = type.create(attrs);
            var transaction = state.tr.insert(position, node);
            dispatch(transaction);
          };
        };
      }
    }, {
      key: "name",
      get: function get() {
        return 'image';
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          inline: true,
          attrs: {
            src: {},
            alt: {
              default: null
            },
            title: {
              default: null
            }
          },
          group: 'inline',
          draggable: true,
          parseDOM: [{
            tag: 'img[src]',
            getAttrs: function getAttrs(dom) {
              return {
                src: dom.getAttribute('src'),
                title: dom.getAttribute('title'),
                alt: dom.getAttribute('alt')
              };
            }
          }],
          toDOM: function toDOM(node) {
            return ['img', node.attrs];
          }
        };
      }
    }, {
      key: "plugins",
      get: function get() {
        return [new tiptap.Plugin({
          props: {
            handleDOMEvents: {
              drop: function drop(view, event) {
                var hasFiles = event.dataTransfer && event.dataTransfer.files && event.dataTransfer.files.length;

                if (!hasFiles) {
                  return;
                }

                var images = Array.from(event.dataTransfer.files).filter(function (file) {
                  return /image/i.test(file.type);
                });

                if (images.length === 0) {
                  return;
                }

                event.preventDefault();
                var schema = view.state.schema;
                var coordinates = view.posAtCoords({
                  left: event.clientX,
                  top: event.clientY
                });
                images.forEach(function (image) {
                  var reader = new FileReader();

                  reader.onload = function (readerEvent) {
                    var node = schema.nodes.image.create({
                      src: readerEvent.target.result
                    });
                    var transaction = view.state.tr.insert(coordinates.pos, node);
                    view.dispatch(transaction);
                  };

                  reader.readAsDataURL(image);
                });
              }
            }
          }
        })];
      }
    }]);

    return Image;
  }(tiptap.Node);

  var ListItem =
  /*#__PURE__*/
  function (_Node) {
    _inherits(ListItem, _Node);

    function ListItem() {
      _classCallCheck(this, ListItem);

      return _possibleConstructorReturn(this, _getPrototypeOf(ListItem).apply(this, arguments));
    }

    _createClass(ListItem, [{
      key: "keys",
      value: function keys(_ref) {
        var type = _ref.type;
        return {
          Enter: tiptapCommands.splitListItem(type),
          Tab: tiptapCommands.sinkListItem(type),
          'Shift-Tab': tiptapCommands.liftListItem(type)
        };
      }
    }, {
      key: "name",
      get: function get() {
        return 'list_item';
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          content: 'paragraph block*',
          defining: true,
          draggable: false,
          parseDOM: [{
            tag: 'li'
          }],
          toDOM: function toDOM() {
            return ['li', 0];
          }
        };
      }
    }]);

    return ListItem;
  }(tiptap.Node);

  function triggerCharacter(_ref) {
    var _ref$char = _ref.char,
        char = _ref$char === void 0 ? '@' : _ref$char,
        _ref$allowSpaces = _ref.allowSpaces,
        allowSpaces = _ref$allowSpaces === void 0 ? false : _ref$allowSpaces,
        _ref$startOfLine = _ref.startOfLine,
        startOfLine = _ref$startOfLine === void 0 ? false : _ref$startOfLine;
    return function ($position) {
      // Matching expressions used for later
      var escapedChar = "\\".concat(char);
      var suffix = new RegExp("\\s".concat(escapedChar, "$"));
      var prefix = startOfLine ? '^' : '';
      var regexp = allowSpaces ? new RegExp("".concat(prefix).concat(escapedChar, ".*?(?=\\s").concat(escapedChar, "|$)"), 'gm') : new RegExp("".concat(prefix, "(?:^)?").concat(escapedChar, "[^\\s").concat(escapedChar, "]*"), 'gm'); // Lookup the boundaries of the current node

      var textFrom = $position.before();
      var textTo = $position.end();
      var text = $position.doc.textBetween(textFrom, textTo, '\0', '\0');
      var match = regexp.exec(text);
      var position;

      while (match !== null) {
        // JavaScript doesn't have lookbehinds; this hacks a check that first character is " "
        // or the line beginning
        var matchPrefix = match.input.slice(Math.max(0, match.index - 1), match.index);

        if (/^[\s\0]?$/.test(matchPrefix)) {
          // The absolute position of the match in the document
          var from = match.index + $position.start();
          var to = from + match[0].length; // Edge case handling; if spaces are allowed and we're directly in between
          // two triggers

          if (allowSpaces && suffix.test(text.slice(to - 1, to + 1))) {
            match[0] += ' ';
            to += 1;
          } // If the $position is located within the matched substring, return that range


          if (from < $position.pos && to >= $position.pos) {
            position = {
              range: {
                from: from,
                to: to
              },
              query: match[0].slice(char.length),
              text: match[0]
            };
          }
        }

        match = regexp.exec(text);
      }

      return position;
    };
  }

  function SuggestionsPlugin(_ref2) {
    var _ref2$matcher = _ref2.matcher,
        matcher = _ref2$matcher === void 0 ? {
      char: '@',
      allowSpaces: false,
      startOfLine: false
    } : _ref2$matcher,
        _ref2$appendText = _ref2.appendText,
        appendText = _ref2$appendText === void 0 ? null : _ref2$appendText,
        _ref2$suggestionClass = _ref2.suggestionClass,
        suggestionClass = _ref2$suggestionClass === void 0 ? 'suggestion' : _ref2$suggestionClass,
        _ref2$command = _ref2.command,
        _command = _ref2$command === void 0 ? function () {
      return false;
    } : _ref2$command,
        _ref2$items = _ref2.items,
        items = _ref2$items === void 0 ? [] : _ref2$items,
        _ref2$onEnter = _ref2.onEnter,
        onEnter = _ref2$onEnter === void 0 ? function () {
      return false;
    } : _ref2$onEnter,
        _ref2$onChange = _ref2.onChange,
        onChange = _ref2$onChange === void 0 ? function () {
      return false;
    } : _ref2$onChange,
        _ref2$onExit = _ref2.onExit,
        onExit = _ref2$onExit === void 0 ? function () {
      return false;
    } : _ref2$onExit,
        _ref2$onKeyDown = _ref2.onKeyDown,
        onKeyDown = _ref2$onKeyDown === void 0 ? function () {
      return false;
    } : _ref2$onKeyDown,
        _ref2$onFilter = _ref2.onFilter,
        onFilter = _ref2$onFilter === void 0 ? function (searchItems, query) {
      if (!query) {
        return searchItems;
      }

      return searchItems.filter(function (item) {
        return JSON.stringify(item).toLowerCase().includes(query.toLowerCase());
      });
    } : _ref2$onFilter;

    return new prosemirrorState.Plugin({
      key: new prosemirrorState.PluginKey('suggestions'),
      view: function view() {
        var _this = this;

        return {
          update: function update(view, prevState) {
            var prev = _this.key.getState(prevState);

            var next = _this.key.getState(view.state); // See how the state changed


            var moved = prev.active && next.active && prev.range.from !== next.range.from;
            var started = !prev.active && next.active;
            var stopped = prev.active && !next.active;
            var changed = !started && !stopped && prev.query !== next.query;
            var handleStart = started || moved;
            var handleChange = changed && !moved;
            var handleExit = stopped || moved; // Cancel when suggestion isn't active

            if (!handleStart && !handleChange && !handleExit) {
              return;
            }

            var state = handleExit ? prev : next;
            var decorationNode = document.querySelector("[data-decoration-id=\"".concat(state.decorationId, "\"]")); // build a virtual node for popper.js or tippy.js
            // this can be used for building popups without a DOM node

            var virtualNode = decorationNode ? {
              getBoundingClientRect: function getBoundingClientRect() {
                return decorationNode.getBoundingClientRect();
              },
              clientWidth: decorationNode.clientWidth,
              clientHeight: decorationNode.clientHeight
            } : null;
            var props = {
              view: view,
              range: state.range,
              query: state.query,
              text: state.text,
              decorationNode: decorationNode,
              virtualNode: virtualNode,
              items: onFilter(Array.isArray(items) ? items : items(), state.query),
              command: function command(_ref3) {
                var range = _ref3.range,
                    attrs = _ref3.attrs;

                _command({
                  range: range,
                  attrs: attrs,
                  schema: view.state.schema
                })(view.state, view.dispatch, view);

                if (appendText) {
                  tiptapCommands.insertText(appendText)(view.state, view.dispatch, view);
                }
              } // Trigger the hooks when necessary

            };

            if (handleExit) {
              onExit(props);
            }

            if (handleChange) {
              onChange(props);
            }

            if (handleStart) {
              onEnter(props);
            }
          }
        };
      },
      state: {
        // Initialize the plugin's internal state.
        init: function init() {
          return {
            active: false,
            range: {},
            query: null,
            text: null
          };
        },
        // Apply changes to the plugin state from a view transaction.
        apply: function apply(tr, prev) {
          var selection = tr.selection;

          var next = _objectSpread({}, prev); // We can only be suggesting if there is no selection


          if (selection.from === selection.to) {
            // Reset active state if we just left the previous suggestion range
            if (selection.from < prev.range.from || selection.from > prev.range.to) {
              next.active = false;
            } // Try to match against where our cursor currently is


            var $position = selection.$from;
            var match = triggerCharacter(matcher)($position);
            var decorationId = (Math.random() + 1).toString(36).substr(2, 5); // If we found a match, update the current state to show it

            if (match) {
              next.active = true;
              next.decorationId = prev.decorationId ? prev.decorationId : decorationId;
              next.range = match.range;
              next.query = match.query;
              next.text = match.text;
            } else {
              next.active = false;
            }
          } else {
            next.active = false;
          } // Make sure to empty the range if suggestion is inactive


          if (!next.active) {
            next.decorationId = null;
            next.range = {};
            next.query = null;
            next.text = null;
          }

          return next;
        }
      },
      props: {
        // Call the keydown hook if suggestion is active.
        handleKeyDown: function handleKeyDown(view, event) {
          var _this$getState = this.getState(view.state),
              active = _this$getState.active,
              range = _this$getState.range;

          if (!active) return false;
          return onKeyDown({
            view: view,
            event: event,
            range: range
          });
        },
        // Setup decorator on the currently active suggestion.
        decorations: function decorations(editorState) {
          var _this$getState2 = this.getState(editorState),
              active = _this$getState2.active,
              range = _this$getState2.range,
              decorationId = _this$getState2.decorationId;

          if (!active) return null;
          return prosemirrorView.DecorationSet.create(editorState.doc, [prosemirrorView.Decoration.inline(range.from, range.to, {
            nodeName: 'span',
            class: suggestionClass,
            'data-decoration-id': decorationId
          })]);
        }
      }
    });
  }

  var Mention =
  /*#__PURE__*/
  function (_Node) {
    _inherits(Mention, _Node);

    function Mention() {
      _classCallCheck(this, Mention);

      return _possibleConstructorReturn(this, _getPrototypeOf(Mention).apply(this, arguments));
    }

    _createClass(Mention, [{
      key: "commands",
      value: function commands(_ref) {
        var _this = this;

        var schema = _ref.schema;
        return function (attrs) {
          return tiptapCommands.replaceText(null, schema.nodes[_this.name], attrs);
        };
      }
    }, {
      key: "name",
      get: function get() {
        return 'mention';
      }
    }, {
      key: "defaultOptions",
      get: function get() {
        return {
          matcher: {
            char: '@',
            allowSpaces: false,
            startOfLine: false
          },
          mentionClass: 'mention',
          suggestionClass: 'mention-suggestion'
        };
      }
    }, {
      key: "schema",
      get: function get() {
        var _this2 = this;

        return {
          attrs: {
            id: {},
            label: {}
          },
          group: 'inline',
          inline: true,
          selectable: false,
          atom: true,
          toDOM: function toDOM(node) {
            return ['span', {
              class: _this2.options.mentionClass,
              'data-mention-id': node.attrs.id
            }, "".concat(_this2.options.matcher.char).concat(node.attrs.label)];
          },
          parseDOM: [{
            tag: 'span[data-mention-id]',
            getAttrs: function getAttrs(dom) {
              var id = dom.getAttribute('data-mention-id');
              var label = dom.innerText.split(_this2.options.matcher.char).join('');
              return {
                id: id,
                label: label
              };
            }
          }]
        };
      }
    }, {
      key: "plugins",
      get: function get() {
        var _this3 = this;

        return [SuggestionsPlugin({
          command: function command(_ref2) {
            var range = _ref2.range,
                attrs = _ref2.attrs,
                schema = _ref2.schema;
            return tiptapCommands.replaceText(range, schema.nodes[_this3.name], attrs);
          },
          appendText: ' ',
          matcher: this.options.matcher,
          items: this.options.items,
          onEnter: this.options.onEnter,
          onChange: this.options.onChange,
          onExit: this.options.onExit,
          onKeyDown: this.options.onKeyDown,
          onFilter: this.options.onFilter,
          suggestionClass: this.options.suggestionClass
        })];
      }
    }]);

    return Mention;
  }(tiptap.Node);

  var OrderedList =
  /*#__PURE__*/
  function (_Node) {
    _inherits(OrderedList, _Node);

    function OrderedList() {
      _classCallCheck(this, OrderedList);

      return _possibleConstructorReturn(this, _getPrototypeOf(OrderedList).apply(this, arguments));
    }

    _createClass(OrderedList, [{
      key: "commands",
      value: function commands(_ref) {
        var type = _ref.type,
            schema = _ref.schema;
        return function () {
          return tiptapCommands.toggleList(type, schema.nodes.list_item);
        };
      }
    }, {
      key: "keys",
      value: function keys(_ref2) {
        var type = _ref2.type,
            schema = _ref2.schema;
        return {
          'Shift-Ctrl-9': tiptapCommands.toggleList(type, schema.nodes.list_item)
        };
      }
    }, {
      key: "inputRules",
      value: function inputRules(_ref3) {
        var type = _ref3.type;
        return [tiptapCommands.wrappingInputRule(/^(\d+)\.\s$/, type, function (match) {
          return {
            order: +match[1]
          };
        }, function (match, node) {
          return node.childCount + node.attrs.order === +match[1];
        })];
      }
    }, {
      key: "name",
      get: function get() {
        return 'ordered_list';
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          attrs: {
            order: {
              default: 1
            }
          },
          content: 'list_item+',
          group: 'block',
          parseDOM: [{
            tag: 'ol',
            getAttrs: function getAttrs(dom) {
              return {
                order: dom.hasAttribute('start') ? +dom.getAttribute('start') : 1
              };
            }
          }],
          toDOM: function toDOM(node) {
            return node.attrs.order === 1 ? ['ol', 0] : ['ol', {
              start: node.attrs.order
            }, 0];
          }
        };
      }
    }]);

    return OrderedList;
  }(tiptap.Node);

  var TableNodes = prosemirrorTables.tableNodes({
    tableGroup: 'block',
    cellContent: 'block+',
    cellAttributes: {
      background: {
        default: null,
        getFromDOM: function getFromDOM(dom) {
          return dom.style.backgroundColor || null;
        },
        setDOMAttr: function setDOMAttr(value, attrs) {
          if (value) {
            var style = {
              style: "".concat(attrs.style || '', "background-color: ").concat(value, ";")
            };
            Object.assign(attrs, style);
          }
        }
      }
    }
  });

  var Table =
  /*#__PURE__*/
  function (_Node) {
    _inherits(Table, _Node);

    function Table() {
      _classCallCheck(this, Table);

      return _possibleConstructorReturn(this, _getPrototypeOf(Table).apply(this, arguments));
    }

    _createClass(Table, [{
      key: "commands",
      value: function commands(_ref) {
        var schema = _ref.schema;
        return {
          createTable: function createTable(_ref2) {
            var rowsCount = _ref2.rowsCount,
                colsCount = _ref2.colsCount,
                withHeaderRow = _ref2.withHeaderRow;
            return function (state, dispatch) {
              var nodes = prosemirrorUtils.createTable(schema, rowsCount, colsCount, withHeaderRow);

              var tr = state.tr.replaceSelectionWith(nodes).scrollIntoView();
              dispatch(tr);
            };
          },
          addColumnBefore: function addColumnBefore() {
            return prosemirrorTables.addColumnBefore;
          },
          addColumnAfter: function addColumnAfter() {
            return prosemirrorTables.addColumnAfter;
          },
          deleteColumn: function deleteColumn() {
            return prosemirrorTables.deleteColumn;
          },
          addRowBefore: function addRowBefore() {
            return prosemirrorTables.addRowBefore;
          },
          addRowAfter: function addRowAfter() {
            return prosemirrorTables.addRowAfter;
          },
          deleteRow: function deleteRow() {
            return prosemirrorTables.deleteRow;
          },
          deleteTable: function deleteTable() {
            return prosemirrorTables.deleteTable;
          },
          toggleCellMerge: function toggleCellMerge() {
            return function (state, dispatch) {
              if (prosemirrorTables.mergeCells(state, dispatch)) {
                return;
              }

              prosemirrorTables.splitCell(state, dispatch);
            };
          },
          mergeCells: function mergeCells() {
            return prosemirrorTables.mergeCells;
          },
          splitCell: function splitCell() {
            return prosemirrorTables.splitCell;
          },
          toggleHeaderColumn: function toggleHeaderColumn() {
            return prosemirrorTables.toggleHeaderColumn;
          },
          toggleHeaderRow: function toggleHeaderRow() {
            return prosemirrorTables.toggleHeaderRow;
          },
          toggleHeaderCell: function toggleHeaderCell() {
            return prosemirrorTables.toggleHeaderCell;
          },
          setCellAttr: function setCellAttr() {
            return prosemirrorTables.setCellAttr;
          },
          fixTables: function fixTables() {
            return prosemirrorTables.fixTables;
          }
        };
      }
    }, {
      key: "keys",
      value: function keys() {
        return {
          Tab: prosemirrorTables.goToNextCell(1),
          'Shift-Tab': prosemirrorTables.goToNextCell(-1)
        };
      }
    }, {
      key: "name",
      get: function get() {
        return 'table';
      }
    }, {
      key: "schema",
      get: function get() {
        return TableNodes.table;
      }
    }, {
      key: "plugins",
      get: function get() {
        return [prosemirrorTables.columnResizing(), prosemirrorTables.tableEditing()];
      }
    }]);

    return Table;
  }(tiptap.Node);

  var TableHeader =
  /*#__PURE__*/
  function (_Node) {
    _inherits(TableHeader, _Node);

    function TableHeader() {
      _classCallCheck(this, TableHeader);

      return _possibleConstructorReturn(this, _getPrototypeOf(TableHeader).apply(this, arguments));
    }

    _createClass(TableHeader, [{
      key: "name",
      get: function get() {
        return 'table_header';
      }
    }, {
      key: "schema",
      get: function get() {
        return TableNodes.table_header;
      }
    }]);

    return TableHeader;
  }(tiptap.Node);

  var TableCell =
  /*#__PURE__*/
  function (_Node) {
    _inherits(TableCell, _Node);

    function TableCell() {
      _classCallCheck(this, TableCell);

      return _possibleConstructorReturn(this, _getPrototypeOf(TableCell).apply(this, arguments));
    }

    _createClass(TableCell, [{
      key: "name",
      get: function get() {
        return 'table_cell';
      }
    }, {
      key: "schema",
      get: function get() {
        return TableNodes.table_cell;
      }
    }]);

    return TableCell;
  }(tiptap.Node);

  var TableRow =
  /*#__PURE__*/
  function (_Node) {
    _inherits(TableRow, _Node);

    function TableRow() {
      _classCallCheck(this, TableRow);

      return _possibleConstructorReturn(this, _getPrototypeOf(TableRow).apply(this, arguments));
    }

    _createClass(TableRow, [{
      key: "name",
      get: function get() {
        return 'table_row';
      }
    }, {
      key: "schema",
      get: function get() {
        return TableNodes.table_row;
      }
    }]);

    return TableRow;
  }(tiptap.Node);

  var TodoItem =
  /*#__PURE__*/
  function (_Node) {
    _inherits(TodoItem, _Node);

    function TodoItem() {
      _classCallCheck(this, TodoItem);

      return _possibleConstructorReturn(this, _getPrototypeOf(TodoItem).apply(this, arguments));
    }

    _createClass(TodoItem, [{
      key: "keys",
      value: function keys(_ref) {
        var type = _ref.type;
        return {
          Enter: tiptapCommands.splitToDefaultListItem(type),
          'Shift-Tab': tiptapCommands.liftListItem(type)
        };
      }
    }, {
      key: "name",
      get: function get() {
        return 'todo_item';
      }
    }, {
      key: "view",
      get: function get() {
        return {
          props: ['node', 'updateAttrs', 'editable'],
          methods: {
            onChange: function onChange() {
              this.updateAttrs({
                done: !this.node.attrs.done
              });
            }
          },
          template: "\n        <li data-type=\"todo_item\" :data-done=\"node.attrs.done.toString()\">\n          <span class=\"todo-checkbox\" contenteditable=\"false\" @click=\"onChange\"></span>\n          <div class=\"todo-content\" ref=\"content\" :contenteditable=\"editable.toString()\"></div>\n        </li>\n      "
        };
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          attrs: {
            done: {
              default: false
            }
          },
          draggable: true,
          content: 'paragraph',
          toDOM: function toDOM(node) {
            var done = node.attrs.done;
            return ['li', {
              'data-type': 'todo_item',
              'data-done': done.toString()
            }, ['span', {
              class: 'todo-checkbox',
              contenteditable: 'false'
            }], ['div', {
              class: 'todo-content'
            }, 0]];
          },
          parseDOM: [{
            priority: 51,
            tag: '[data-type="todo_item"]',
            getAttrs: function getAttrs(dom) {
              return {
                done: dom.getAttribute('data-done') === 'true'
              };
            }
          }]
        };
      }
    }]);

    return TodoItem;
  }(tiptap.Node);

  var TodoList =
  /*#__PURE__*/
  function (_Node) {
    _inherits(TodoList, _Node);

    function TodoList() {
      _classCallCheck(this, TodoList);

      return _possibleConstructorReturn(this, _getPrototypeOf(TodoList).apply(this, arguments));
    }

    _createClass(TodoList, [{
      key: "commands",
      value: function commands(_ref) {
        var type = _ref.type;
        return function () {
          return tiptapCommands.wrapInList(type);
        };
      }
    }, {
      key: "inputRules",
      value: function inputRules(_ref2) {
        var type = _ref2.type;
        return [tiptapCommands.wrappingInputRule(/^\s*(\[ \])\s$/, type)];
      }
    }, {
      key: "name",
      get: function get() {
        return 'todo_list';
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          group: 'block',
          content: 'todo_item+',
          toDOM: function toDOM() {
            return ['ul', {
              'data-type': 'todo_list'
            }, 0];
          },
          parseDOM: [{
            priority: 51,
            tag: '[data-type="todo_list"]'
          }]
        };
      }
    }]);

    return TodoList;
  }(tiptap.Node);

  var Bold =
  /*#__PURE__*/
  function (_Mark) {
    _inherits(Bold, _Mark);

    function Bold() {
      _classCallCheck(this, Bold);

      return _possibleConstructorReturn(this, _getPrototypeOf(Bold).apply(this, arguments));
    }

    _createClass(Bold, [{
      key: "keys",
      value: function keys(_ref) {
        var type = _ref.type;
        return {
          'Mod-b': tiptapCommands.toggleMark(type)
        };
      }
    }, {
      key: "commands",
      value: function commands(_ref2) {
        var type = _ref2.type;
        return function () {
          return tiptapCommands.toggleMark(type);
        };
      }
    }, {
      key: "inputRules",
      value: function inputRules(_ref3) {
        var type = _ref3.type;
        return [tiptapCommands.markInputRule(/(?:\*\*|__)([^*_]+)(?:\*\*|__)$/, type)];
      }
    }, {
      key: "name",
      get: function get() {
        return 'bold';
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          parseDOM: [{
            tag: 'strong'
          }, {
            tag: 'b',
            getAttrs: function getAttrs(node) {
              return node.style.fontWeight !== 'normal' && null;
            }
          }, {
            style: 'font-weight',
            getAttrs: function getAttrs(value) {
              return /^(bold(er)?|[5-9]\d{2,})$/.test(value) && null;
            }
          }],
          toDOM: function toDOM() {
            return ['strong', 0];
          }
        };
      }
    }]);

    return Bold;
  }(tiptap.Mark);

  var Code =
  /*#__PURE__*/
  function (_Mark) {
    _inherits(Code, _Mark);

    function Code() {
      _classCallCheck(this, Code);

      return _possibleConstructorReturn(this, _getPrototypeOf(Code).apply(this, arguments));
    }

    _createClass(Code, [{
      key: "keys",
      value: function keys(_ref) {
        var type = _ref.type;
        return {
          'Mod-`': tiptapCommands.toggleMark(type)
        };
      }
    }, {
      key: "commands",
      value: function commands(_ref2) {
        var type = _ref2.type;
        return function () {
          return tiptapCommands.toggleMark(type);
        };
      }
    }, {
      key: "inputRules",
      value: function inputRules(_ref3) {
        var type = _ref3.type;
        return [tiptapCommands.markInputRule(/(?:`)([^`]+)(?:`)$/, type)];
      }
    }, {
      key: "name",
      get: function get() {
        return 'code';
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          parseDOM: [{
            tag: 'code'
          }],
          toDOM: function toDOM() {
            return ['code', 0];
          }
        };
      }
    }]);

    return Code;
  }(tiptap.Mark);

  var Italic =
  /*#__PURE__*/
  function (_Mark) {
    _inherits(Italic, _Mark);

    function Italic() {
      _classCallCheck(this, Italic);

      return _possibleConstructorReturn(this, _getPrototypeOf(Italic).apply(this, arguments));
    }

    _createClass(Italic, [{
      key: "keys",
      value: function keys(_ref) {
        var type = _ref.type;
        return {
          'Mod-i': tiptapCommands.toggleMark(type)
        };
      }
    }, {
      key: "commands",
      value: function commands(_ref2) {
        var type = _ref2.type;
        return function () {
          return tiptapCommands.toggleMark(type);
        };
      }
    }, {
      key: "inputRules",
      value: function inputRules(_ref3) {
        var type = _ref3.type;
        return [tiptapCommands.markInputRule(/(?:^|[^*_])(?:\*|_)([^*_]+)(?:\*|_)$/, type)];
      }
    }, {
      key: "name",
      get: function get() {
        return 'italic';
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          parseDOM: [{
            tag: 'i'
          }, {
            tag: 'em'
          }, {
            style: 'font-style=italic'
          }],
          toDOM: function toDOM() {
            return ['em', 0];
          }
        };
      }
    }]);

    return Italic;
  }(tiptap.Mark);

  var Link =
  /*#__PURE__*/
  function (_Mark) {
    _inherits(Link, _Mark);

    function Link() {
      _classCallCheck(this, Link);

      return _possibleConstructorReturn(this, _getPrototypeOf(Link).apply(this, arguments));
    }

    _createClass(Link, [{
      key: "commands",
      value: function commands(_ref) {
        var type = _ref.type;
        return function (attrs) {
          if (attrs.href) {
            return tiptapCommands.updateMark(type, attrs);
          }

          return tiptapCommands.removeMark(type);
        };
      }
    }, {
      key: "pasteRules",
      value: function pasteRules(_ref2) {
        var type = _ref2.type;
        return [tiptapCommands.pasteRule(/https?:\/\/(www\.)?[-a-zA-Z0-9@:%._+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_+.~#?&//=]*)/g, type, function (url) {
          return {
            href: url
          };
        })];
      }
    }, {
      key: "name",
      get: function get() {
        return 'link';
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          attrs: {
            href: {
              default: null
            }
          },
          inclusive: false,
          parseDOM: [{
            tag: 'a[href]',
            getAttrs: function getAttrs(dom) {
              return {
                href: dom.getAttribute('href')
              };
            }
          }],
          toDOM: function toDOM(node) {
            return ['a', _objectSpread({}, node.attrs, {
              rel: 'noopener noreferrer nofollow'
            }), 0];
          }
        };
      }
    }, {
      key: "plugins",
      get: function get() {
        return [new tiptap.Plugin({
          props: {
            handleClick: function handleClick(view, pos) {
              var _view$state = view.state,
                  schema = _view$state.schema,
                  doc = _view$state.doc,
                  tr = _view$state.tr;
              var range = tiptapUtils.getMarkRange(doc.resolve(pos), schema.marks.link);

              if (!range) {
                return;
              }

              var $start = doc.resolve(range.from);
              var $end = doc.resolve(range.to);
              var transaction = tr.setSelection(new tiptap.TextSelection($start, $end));
              view.dispatch(transaction);
            }
          }
        })];
      }
    }]);

    return Link;
  }(tiptap.Mark);

  var Strike =
  /*#__PURE__*/
  function (_Mark) {
    _inherits(Strike, _Mark);

    function Strike() {
      _classCallCheck(this, Strike);

      return _possibleConstructorReturn(this, _getPrototypeOf(Strike).apply(this, arguments));
    }

    _createClass(Strike, [{
      key: "keys",
      value: function keys(_ref) {
        var type = _ref.type;
        return {
          'Mod-d': tiptapCommands.toggleMark(type)
        };
      }
    }, {
      key: "commands",
      value: function commands(_ref2) {
        var type = _ref2.type;
        return function () {
          return tiptapCommands.toggleMark(type);
        };
      }
    }, {
      key: "inputRules",
      value: function inputRules(_ref3) {
        var type = _ref3.type;
        return [tiptapCommands.markInputRule(/~([^~]+)~$/, type)];
      }
    }, {
      key: "name",
      get: function get() {
        return 'strike';
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          parseDOM: [{
            tag: 's'
          }, {
            tag: 'del'
          }, {
            tag: 'strike'
          }, {
            style: 'text-decoration',
            getAttrs: function getAttrs(value) {
              return value === 'line-through';
            }
          }],
          toDOM: function toDOM() {
            return ['s', 0];
          }
        };
      }
    }]);

    return Strike;
  }(tiptap.Mark);

  var Underline =
  /*#__PURE__*/
  function (_Mark) {
    _inherits(Underline, _Mark);

    function Underline() {
      _classCallCheck(this, Underline);

      return _possibleConstructorReturn(this, _getPrototypeOf(Underline).apply(this, arguments));
    }

    _createClass(Underline, [{
      key: "keys",
      value: function keys(_ref) {
        var type = _ref.type;
        return {
          'Mod-u': tiptapCommands.toggleMark(type)
        };
      }
    }, {
      key: "commands",
      value: function commands(_ref2) {
        var type = _ref2.type;
        return function () {
          return tiptapCommands.toggleMark(type);
        };
      }
    }, {
      key: "name",
      get: function get() {
        return 'underline';
      }
    }, {
      key: "schema",
      get: function get() {
        return {
          parseDOM: [{
            tag: 'u'
          }, {
            style: 'text-decoration',
            getAttrs: function getAttrs(value) {
              return value === 'underline';
            }
          }],
          toDOM: function toDOM() {
            return ['u', 0];
          }
        };
      }
    }]);

    return Underline;
  }(tiptap.Mark);

  var History =
  /*#__PURE__*/
  function (_Extension) {
    _inherits(History, _Extension);

    function History() {
      _classCallCheck(this, History);

      return _possibleConstructorReturn(this, _getPrototypeOf(History).apply(this, arguments));
    }

    _createClass(History, [{
      key: "keys",
      value: function keys() {
        var isMac = typeof navigator !== 'undefined' ? /Mac/.test(navigator.platform) : false;
        var keymap = {
          'Mod-z': prosemirrorHistory.undo,
          'Shift-Mod-z': prosemirrorHistory.redo
        };

        if (!isMac) {
          keymap['Mod-y'] = prosemirrorHistory.redo;
        }

        return keymap;
      }
    }, {
      key: "commands",
      value: function commands() {
        return {
          undo: function undo() {
            return prosemirrorHistory.undo;
          },
          redo: function redo() {
            return prosemirrorHistory.redo;
          }
        };
      }
    }, {
      key: "name",
      get: function get() {
        return 'history';
      }
    }, {
      key: "plugins",
      get: function get() {
        return [prosemirrorHistory.history()];
      }
    }]);

    return History;
  }(tiptap.Extension);

  var Placeholder =
  /*#__PURE__*/
  function (_Extension) {
    _inherits(Placeholder, _Extension);

    function Placeholder() {
      _classCallCheck(this, Placeholder);

      return _possibleConstructorReturn(this, _getPrototypeOf(Placeholder).apply(this, arguments));
    }

    _createClass(Placeholder, [{
      key: "name",
      get: function get() {
        return 'placeholder';
      }
    }, {
      key: "defaultOptions",
      get: function get() {
        return {
          emptyNodeClass: 'is-empty',
          emptyNodeText: 'Write something...'
        };
      }
    }, {
      key: "plugins",
      get: function get() {
        var _this = this;

        return [new tiptap.Plugin({
          props: {
            decorations: function decorations(_ref) {
              var doc = _ref.doc;
              var decorations = [];
              var completelyEmpty = doc.textContent === '' && doc.childCount <= 1 && doc.content.size <= 2;
              doc.descendants(function (node, pos) {
                if (!completelyEmpty) {
                  return;
                }

                var decoration = prosemirrorView.Decoration.node(pos, pos + node.nodeSize, {
                  class: _this.options.emptyNodeClass,
                  'data-empty-text': _this.options.emptyNodeText
                });
                decorations.push(decoration);
              });
              return prosemirrorView.DecorationSet.create(doc, decorations);
            }
          }
        })];
      }
    }]);

    return Placeholder;
  }(tiptap.Extension);

  exports.Blockquote = Blockquote;
  exports.BulletList = Bullet;
  exports.CodeBlock = CodeBlock;
  exports.CodeBlockHighlight = CodeBlockHighlight;
  exports.HardBreak = HardBreak;
  exports.Heading = Heading;
  exports.Image = Image;
  exports.ListItem = ListItem;
  exports.Mention = Mention;
  exports.OrderedList = OrderedList;
  exports.Table = Table;
  exports.TableHeader = TableHeader;
  exports.TableCell = TableCell;
  exports.TableRow = TableRow;
  exports.TodoItem = TodoItem;
  exports.TodoList = TodoList;
  exports.Bold = Bold;
  exports.Code = Code;
  exports.Italic = Italic;
  exports.Link = Link;
  exports.Strike = Strike;
  exports.Underline = Underline;
  exports.History = History;
  exports.Placeholder = Placeholder;
  exports.Suggestions = SuggestionsPlugin;

  Object.defineProperty(exports, '__esModule', { value: true });

}));
