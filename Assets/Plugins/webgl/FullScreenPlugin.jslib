var FullScreenPlugin = {
     FullScreenFunction: function()
     {
         document.makeFullscreen('unity-container');
     },
 };
 mergeInto(LibraryManager.library, FullScreenPlugin);