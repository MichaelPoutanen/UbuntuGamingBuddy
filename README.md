This is a simple tool that helps playing games on services like ShadowPC more comfortable.

Since I am no longer having a beefy gaming PC, I use ShadowPC to play games - but there were always two issues that bugged me:

The first one being, that I couldn't alt+tab inside of that VM without switching windows on my Ubuntu. The second one was, that sporadically the "This application doesn't respond" window came up, even though it worked just fine - and one wrong key press there and you'd kill it.

So, I made this small program, which basically is a wrapper around a few console commands that disables both - that way you can alt-tab on the VM and it won't come nagging and telling you that the window doesn't react any longer.


You can also re-enable the warnings again by pressing "Disable gaming mode" (or right-click on the tray icon and toggle game mode).



A little warning: I made this specifically to my needs, so if you use some special configuration in your Gnome/Ubuntu-Settings, this might mess it up - I will add a "Backup settings on first start"-Thingy soon, though, so that it will always reset to your personal preferences instead of what I use (system standard).
