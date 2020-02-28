
      var st=document.getElementsByTagName('script');
      var au=st[st.length - 1].src;
      var bp = au.replace('apic-import.js', '');
      var legacyScriptsCount = 0;
      function styleDocument() {
        document.addEventListener('WebComponentsReady', function() {
          setTimeout(function() {
            function shouldAddDocumentStyle(n) {
              return n.nodeType === Node.ELEMENT_NODE && n.localName === 'style' && !n.hasAttribute('scope');
            }
            const CustomStyleInterface = window.ShadyCSS.CustomStyleInterface;
            const candidates = document.querySelectorAll('style');
            for (let i = 0; i < candidates.length; i++) {
              const candidate = candidates[i];
              if (shouldAddDocumentStyle(candidate)) {
                CustomStyleInterface.addCustomStyle(candidate);
              }
            }
          }, 1000);
        });
      }
      function loadConsoleWhenDone(){
        legacyScriptsCount++
        if(legacyScriptsCount==3 && isLegacy()==true){
          loadConsole();
          styleDocument();
        }
      }
      function addScript(src, onLoadCallback) {
        var s = document.createElement('script');
        s.setAttribute('nomodule', '');
        s.src = bp + src;
        s.onload = onLoadCallback;
        document.body.appendChild(s);
      }
    addScript('polyfills/core-js.75d89a608ee8c655883ccf374ca31995.js',loadConsoleWhenDone);addScript('polyfills/systemjs.6dfbfd8f2c3e558918ed74d133a6757a.js',loadConsoleWhenDone);addScript('polyfills/regenerator-runtime.323cb013cc2a9c88ff67ee256cbf5942.js',loadConsoleWhenDone);function loadConsole() {try{s = document.createElement('script');s.innerHTML = 'window.importShim = s => import(s)';document.body.appendChild(s);}catch(e){console.log(e);};try{!function(){function e(t,n){return new Promise(function(e,o){document.head.appendChild(Object.assign(document.createElement("script"),{src:bp+t,onload:e,onerror:o},n?{type:"module"}:void 0))})}var o=[];function t(){"noModule"in HTMLScriptElement.prototype?window.importShim(bp+"api-console-73ee4a86.js"):System.import(bp+"legacy/api-console-dfa6967e.js")}"fetch"in window||o.push(e("polyfills/fetch.e0fa1d30ce1c9b23c0898a2e34c3fe3b.js",!1)),"noModule"in HTMLScriptElement.prototype&&!("importShim"in window)&&o.push(e("polyfills/dynamic-import.b745cfc9384367cc18b42bbef2bbdcd9.js",!1)),"attachShadow"in Element.prototype&&"getRootNode"in Element.prototype&&(!window.ShadyDOM||!window.ShadyDOM.force)||o.push(e("polyfills/webcomponents.dae9f79d9d6992b6582e204c3dd953d3.js",!1)),!("noModule"in HTMLScriptElement.prototype)&&"getRootNode"in Element.prototype&&o.push(e("polyfills/custom-elements-es5-adapter.84b300ee818dce8b351c7cc7c100bcf7.js",!1)),o.length?Promise.all(o).then(t):t()}()}catch(e){console.log(e);};}
    function isLegacy() {
      const script = document.createElement('script');
      return !('noModule' in script);
    }
    if(isLegacy()==false){
      loadConsole();
    }
    