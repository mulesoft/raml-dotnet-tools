'use strict'
/* global describe, it */

var should = require('should')
var LRUCache = require('../lrucache.js')
var x = {a: 1, b: 2}

describe('LRUCache', function () {

  it('LRUCache()', function () {
    var cache = LRUCache(100)
    should(cache.info().length).be.equal(0)
    should(cache.info().capacity).be.equal(100)
    should(cache.has('a')).be.equal(false)
    should(cache.get('a')).be.equal(undefined)
    should().be.equal()

    cache.set('a', x)
    should(cache.info().length).be.equal(1)
    should(cache.has('a')).be.equal(true)
    should(cache.get('a')).be.equal(x)

    cache.set('b', [])
    should(cache.info().length).be.equal(2)
    should(cache.get('b')).be.eql([])

    cache.set('c', null)
    should(cache.info().length).be.equal(3)
    should(cache.get('c')).be.eql(null)

    should(cache.keys()).be.eql(['c', 'b', 'a'])
    cache.get('b')
    should(cache.keys()).be.eql(['b', 'c', 'a'])
    cache.remove('c')
    should(cache.get('c')).be.eql(undefined)
    should(cache.keys()).be.eql(['b', 'a'])

    cache.update('a', function (a) {
      should(a).be.equal(x)
      a.a = 2
      return a
    })
    cache.update('c', function (a) {
      should(true).be.equal(false)
    })

    should(cache.get('a')).be.eql({a: 2, b: 2})
    should(cache.keys()).be.eql(['a', 'b'])
    should(cache.popStale()).be.eql(['b', []])
    should(cache.keys()).be.eql(['a'])
  })
})
