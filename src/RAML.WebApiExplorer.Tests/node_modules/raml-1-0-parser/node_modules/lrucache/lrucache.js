// **Github:** https://github.com/zensh/lrucache
//
// **License:** MIT

/* global module, define */
;(function (root, factory) {
  'use strict'

  if (typeof module === 'object' && typeof module.exports === 'object') {
    module.exports = factory()
  } else if (typeof define === 'function' && define.amd) {
    define([], factory)
  } else {
    root.LRUCache = factory()
  }
}(typeof window === 'object' ? window : this, function () {
  'use strict'

  var undef = void 0

  function LRUCache (capacity) {
    if (!(this instanceof LRUCache)) return new LRUCache(capacity)
    this._LRUCacheState = new CacheState(capacity)
  }

  var proto = LRUCache.prototype

  proto.get = function (key) {
    var state = this._LRUCacheState
    var lruEntry = state.hash[key]
    if (!lruEntry) return
    refresh(state.linkedList, lruEntry)
    return state.data[key]
  }

  proto.set = function (key, value) {
    var state = this._LRUCacheState
    var lruEntry = state.hash[key]
    if (value === undef) return this
    if (!lruEntry) {
      state.hash[key] = new LRUEntry(key)
      state.linkedList.length += 1
      lruEntry = state.hash[key]
    }
    refresh(state.linkedList, lruEntry)
    state.data[key] = value
    if (state.linkedList.length > state.capacity) this.remove(state.linkedList.end.key)
    return this
  }

  proto.update = function (key, parseFn) {
    if (this.has(key)) this.set(key, parseFn(this.get(key)))
    return this
  }

  proto.remove = function (key) {
    var state = this._LRUCacheState
    var lruEntry = state.hash[key]
    if (!lruEntry) return this
    if (lruEntry === state.linkedList.head) state.linkedList.head = lruEntry.p
    if (lruEntry === state.linkedList.end) state.linkedList.end = lruEntry.n
    link(lruEntry.n, lruEntry.p)
    delete state.hash[key]
    delete state.data[key]
    state.linkedList.length -= 1
    return this
  }

  proto.removeAll = function () {
    this._LRUCacheState = new CacheState(this._LRUCacheState.capacity)
    return this
  }

  proto.info = function () {
    var state = this._LRUCacheState
    return {
      capacity: state.capacity,
      length: state.linkedList.length
    }
  }

  proto.keys = function () {
    var keys = []
    var lruEntry = this._LRUCacheState.linkedList.head
    while (lruEntry) {
      keys.push(lruEntry.key)
      lruEntry = lruEntry.p
    }
    return keys
  }

  proto.has = function (key) {
    return !!this._LRUCacheState.hash[key]
  }

  proto.staleKey = function () {
    return this._LRUCacheState.linkedList.end && this._LRUCacheState.linkedList.end.key
  }

  proto.popStale = function () {
    var staleKey = this.staleKey()
    if (!staleKey) return null
    var stale = [staleKey, this._LRUCacheState.data[staleKey]]
    this.remove(staleKey)
    return stale
  }

  function CacheState (capacity) {
    this.capacity = capacity > 0 ? +capacity : Number.MAX_VALUE
    this.data = Object.create ? Object.create(null) : {}
    this.hash = Object.create ? Object.create(null) : {}
    this.linkedList = new LinkedList()
  }

  function LinkedList () {
    this.length = 0
    this.head = null
    this.end = null
  }

  function LRUEntry (key) {
    this.key = key
    this.p = null
    this.n = null
  }

  // 更新链表，把get或put方法操作的key提到链表head，即表示最新
  function refresh (linkedList, entry) {
    if (entry === linkedList.head) return
    if (!linkedList.end) {
      linkedList.end = entry
    } else if (linkedList.end === entry) {
      linkedList.end = entry.n
    }

    link(entry.n, entry.p)
    link(entry, linkedList.head)
    linkedList.head = entry
    linkedList.head.n = null
  }

  // 对两个链表对象建立链接，形成一条链
  function link (nextEntry, prevEntry) {
    if (nextEntry === prevEntry) return
    if (nextEntry) nextEntry.p = prevEntry
    if (prevEntry) prevEntry.n = nextEntry
  }

  LRUCache.NAME = 'LRUCache'
  LRUCache.VERSION = 'v0.3.0'
  return LRUCache
}))
