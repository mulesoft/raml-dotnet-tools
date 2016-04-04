# Eval Loophole [![Build Status](https://travis-ci.org/atom/loophole.svg?branch=master)](https://travis-ci.org/atom/loophole)

This is a hack to enable third-party libraries that depend on a limited subset
of `eval` semantics to work in Atom with a content security policy that forbids
calls to `eval`.

```coffee
{allowUnsafeEval, allowUnsafeNewFunction} = require 'loophole'

allowUnsafeEval ->
  crazyLibrary.exploitLoophole() # allows `eval(...)`

allowUnsafeNewFunction ->
  crazyLibrary.exploitLoophole() # allows `new Function(...)`
```

You can also use the exported `Function` constructor directly:

```coffee
{Function} = require 'loophole'
f = new Function("return 1 + 1;")
```

## How?

`allowUnsafeEval` replaces `eval` with a call to `vm.runInThisContext`, which
won't perfectly emulate `eval` but is good enough in certain circumstances, like
compiling [PEG.js][peg-js] grammars.

`allowUnsafeNewFunction` temporarily replaces `global.Function` with
`loophole.Function`, which passes the source of the desired function to
`vm.runInThisContext`.

## Why?

If there's a loophole, why even enable CSP? It still prevents developers from
accidentally invoking eval with legacy libraries. For example, did you know that
jQuery runs eval when you pass it content with script tags? If you want eval,
you'll need to explicitly ask for it.

[peg-js]: http://pegjs.majda.cz/
