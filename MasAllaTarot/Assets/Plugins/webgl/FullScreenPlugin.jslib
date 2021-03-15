var FullScreenPlugin = {
     FullScreenFunction: function()
     {
         document.makeFullscreen('gameContainer');
     }
 };
 mergeInto(LibraryManager.library, FullScreenPlugin);