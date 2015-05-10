# JSBadgeView
Xamarin / C# port of JaviSoto/JSBadgeView

Please see the [original docs](https://github.com/JaviSoto/JSBadgeView).

I only implemented the default iOS7 look, but you can just supply your own values for old-style buttons (see JaviSoto/JSBadgeView) initializers.

Some usage:
```
JSBadgeView badgeView = new JSBadgeView() {
    BadgeBackgroundColor = UIColor.Red,
    BadgeStrokeColor = UIColor.Red,
    BadgeTextColor = UIColor.White,
    BadgeTextFont = UIFont.FromName("AvenirNext-Regular", 11),
    BadgeAlignment = JSBadgeView.Alignment.TopRight,
    BadgeText = "22"
};
			
SomeOtherView.Add(badgeView);
```

Note, in the current form, you have to specify BadgeStrokeColor.  If you don't it'll have a null ref exception.



