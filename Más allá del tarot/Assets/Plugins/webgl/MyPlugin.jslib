var MyPlugin = {
     IsMobile: function()
     {
         return UnityLoader.SystemInfo.mobile;
     },
	 
	 CheckOrientation: function()
	 {
        var orientation = (screen.orientation || {}).type || screen.mozOrientation || screen.msOrientation;
        
        if(orientation == "portrait-primary")
        {
            return true;
        }
        else
        {
            return false;
        }
    },
	
	CheckOrientationIOS: function()
	 {       
		if (Math.abs(window.orientation) === 90)
		{
		    return false;
		}
		else
        {
            return true;
        }       
    }
 };
 
 mergeInto(LibraryManager.library, MyPlugin);