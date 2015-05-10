using System;
using UIKit;
using CoreGraphics;

// This is essentially a copy from JSBadgeView from an iOS implementation here:
// https://github.com/JaviSoto/JSBadgeView/blob/master/JSBadgeView/JSBadgeView.m
// by Javier Soto (ios@javisoto.es) adboted here under the originating MIT License.

// Currently only supports defaults for iOS7 (flat), but you can hand-supply the 
// properties to get the look you want.

// Copyright 2015, by WannaGo, LLC 
// By Bernie Habermeier
// MIT License, USE AT OWN RISK, etc.

using System.Security.Permissions;
using Foundation;
using System.Drawing;
using CoreText;
using AVFoundation;
using System.Diagnostics;
using Client.Core.Services;

public class JSBadgeView : UIView
{
	public enum Alignment {
		TopLeft = 1,
		TopRight,
		TopCenter,
		CenterLeft,
		CenterRight,
		BottomLeft,
		BottomRight,
		BottomCenter,
		Center
	}

	private string badgeText;
	public string BadgeText { 
		get { return badgeText; } 
		set { badgeText = value; SetNeedsLayout(); } 
	}

	private Alignment badgeAlignment;
	public Alignment BadgeAlignment { 
		get { return badgeAlignment; } 
		set { badgeAlignment = value; SetNeedsLayout(); } 
	}

	private UIColor badgeTextColor;
	public UIColor BadgeTextColor { 
		get { return badgeTextColor; } 
		set { badgeTextColor = value; SetNeedsLayout(); } 
	}

	private CGSize badgeTextShadowOffset;
	public CGSize BadgeTextShadowOffset { 
		get { return badgeTextShadowOffset; } 
		set { badgeTextShadowOffset = value; SetNeedsLayout(); } 
	}

	private UIColor badgeTextShadowColor;
	public UIColor BadgeTextShadowColor { 
		get { return badgeTextShadowColor; } 
		set { badgeTextShadowColor = value; SetNeedsLayout(); } 
	}

	private UIFont badgeTextFont;
	public UIFont BadgeTextFont { 
		get { return badgeTextFont; } 
		set { badgeTextFont = value; SetNeedsLayout(); } 
	}

	private UIColor badgeBackgroundColor;
	public UIColor BadgeBackgroundColor { 
		get { return badgeBackgroundColor; } 
		set { badgeBackgroundColor = value; SetNeedsLayout(); } 
	}

	private UIColor badgeOverlayColor;
	public UIColor BadgeOverlayColor { 
		get { return badgeOverlayColor; } 
		set { badgeOverlayColor = value; SetNeedsLayout(); } 
	}

	private UIColor badgeShadowColor;
	public UIColor BadgeShadowColor { 
		get { return badgeShadowColor; } 
		set { badgeShadowColor = value; SetNeedsLayout(); } 
	}

	private CGSize badgeShadowSize;
	public CGSize BadgeShadowSize { 
		get { return badgeShadowSize; } 
		set { badgeShadowSize = value; SetNeedsLayout(); } 
	}

	private nfloat badgeStrokeWidth;
	public nfloat BadgeStrokeWidth { 
		get { return badgeStrokeWidth; } 
		set { badgeStrokeWidth = value; SetNeedsLayout(); } 
	}

	private UIColor badgeStrokeColor;
	public UIColor BadgeStrokeColor { 
		get { return badgeStrokeColor; } 
		set { badgeStrokeColor = value; SetNeedsLayout(); } 
	}

	private CGPoint badgePositionAdjustment;
	public CGPoint BadgePositionAdjustment { 
		get { return badgePositionAdjustment; } 
		set { badgePositionAdjustment = value; SetNeedsLayout(); } 
	}

	private CGRect frameToPositionInRelationWith;
	public CGRect FrameToPositionInRelationWith { 
		get { return frameToPositionInRelationWith; } 
		set { frameToPositionInRelationWith = value; SetNeedsLayout(); } 
	}

	private nfloat badgeMinWidth;
	public nfloat BadgeMinWidth { 
		get { return badgeMinWidth; } 
		set { badgeMinWidth = value; SetNeedsLayout(); } 
	}

	public JSBadgeView() : base()
	{
		ApplyIOS7Style();
	}

	public JSBadgeView(UIView parentView, Alignment alignment) : base()
	{
		ApplyIOS7Style();

		parentView.Add(this);

	}

	nfloat jsBadgeViewShadowRadius = 1.0f;
	nfloat jsBadgeViewHeight = 16.0f;
	nfloat jsBadgeViewTextSideMargin = 8.0f;
	nfloat jsBadgeViewCornerRadius = 10.0f;

	const bool isUIKitFlatMode = true; // I only care about iOS 7+... 

	void ApplyIOS7Style()
	{
		BackgroundColor = UIColor.Clear;
		BadgeOverlayColor = UIColor.Clear;
		BadgeTextShadowColor = UIColor.Clear;
		BadgeShadowColor = UIColor.Clear;
		BadgeStrokeWidth = 0.0f;
		BadgeStrokeColor = BadgeBackgroundColor;
	}

	private nfloat MarginToDrawInside()
	{
		return BadgeStrokeWidth * 2.0f;
	}

	private CGSize SizeOfTextForCurrentSettings()
	{

		NSString text = new NSString(BadgeText);
		
		return text.GetSizeUsingAttributes(new UIStringAttributes() {
			Font = BadgeTextFont
		});
		
	}

	public override void LayoutSubviews()
	{
		base.LayoutSubviews();
		
		CGRect newFrame = Frame;

		CGRect superviewBounds = FrameToPositionInRelationWith.IsEmpty ?
				Superview.Bounds : 
				FrameToPositionInRelationWith;

		nfloat textWidth = SizeOfTextForCurrentSettings().Width;

		nfloat marginToDrawInside = MarginToDrawInside();
		nfloat viewWidth = (nfloat)Math.Max(
			                   (double)BadgeMinWidth, 
			                   (double)(textWidth + jsBadgeViewTextSideMargin + (marginToDrawInside * 2))
		                   );
		nfloat viewHeight = jsBadgeViewHeight + (marginToDrawInside * 2);
		nfloat superviewWidth = superviewBounds.Size.Width;
		nfloat superviewHeight = superviewBounds.Size.Height;

		nfloat width = 0, height = 0, x = 0 , y = 0;

		width = (nfloat) Math.Max( (double) viewWidth, (double) viewHeight);
		height = viewHeight;

		switch (BadgeAlignment) {
			case Alignment.TopLeft:
				x = -viewWidth / 2.0f;
				y = -viewHeight / 2.0f;
				break;
			case Alignment.TopRight:
				x = superviewWidth - (viewWidth / 2.0f);
				y = -viewHeight / 2.0f;
				break;
			case Alignment.TopCenter:
				x = (superviewWidth - viewWidth) / 2.0f;
				y = -viewHeight / 2.0f;
				break;
			case Alignment.CenterLeft:
				x = -viewWidth / 2.0f;
				y = (superviewHeight - viewHeight) / 2.0f;
				break;
			case Alignment.CenterRight:
				x = superviewWidth - (viewWidth / 2.0f);
				y = (superviewHeight - viewHeight) / 2.0f;
				break;
			case Alignment.BottomLeft:
				x = -viewWidth / 2.0f;
				y = superviewHeight - (viewHeight / 2.0f);
				break;
			case Alignment.BottomRight:
				x = superviewWidth - (viewWidth / 2.0f);
				y = superviewHeight - (viewHeight / 2.0f);
				break;
			case Alignment.BottomCenter:
				x = (superviewWidth - viewWidth) / 2.0f;
				y = superviewHeight - (viewHeight / 2.0f);
				break;
			case Alignment.Center:
				x = (superviewWidth - viewWidth) / 2.0f;
				y = (superviewHeight - viewHeight) / 2.0f;
				break;
			default:
				Debug.Assert(false, "Unimplemented JSBadgeAligment type {0}", BadgeAlignment.ToString());
				break;
		}

		x += BadgePositionAdjustment.X;
		y += BadgePositionAdjustment.Y;

		// Do not set frame directly so we do not interfere with any potential transform set on the view.
		Bounds = new CGRect(0, 0, width, height).Integral();

		var frame = new CGRect(x, y, width, height);
		Center = new CGPoint(Math.Ceiling((double) frame.GetMidX()), Math.Ceiling((double) frame.GetMidY()));

		SetNeedsDisplay();

	}

	public override void Draw(CGRect rect)
	{
		bool noTextToDraw = BadgeText == null || (BadgeText.Length <= 0);
		if (noTextToDraw) return;

		var ctx = UIGraphics.GetCurrentContext();

		nfloat marginToDrawInside = MarginToDrawInside();

		var region = new RectangleF(
			(float) rect.X, 
			(float) rect.Y, 
			(float) rect.Width, 
			(float) rect.Height);
		
		var inflated = RectangleF.Inflate(region, (float)marginToDrawInside, (float)marginToDrawInside);
		CGRect rectToDraw = new CGRect(inflated.X, inflated.Y, inflated.Width, inflated.Height);

		var borderPath = UIBezierPath.FromRoundedRect(
			                 rect: rectToDraw, 
			                 corners: UIRectCorner.AllCorners, 
			                 radii: new CGSize(jsBadgeViewCornerRadius, jsBadgeViewCornerRadius));

		/* Background and shadow */
		ctx.SaveState();
		{
			ctx.AddPath(borderPath.CGPath);
			ctx.SetFillColor(BadgeBackgroundColor.CGColor);
			ctx.SetShadow(BadgeShadowSize, jsBadgeViewShadowRadius, BadgeShadowColor.CGColor);
			ctx.DrawPath(CGPathDrawingMode.Fill);
		}
		ctx.RestoreState();

		/* Gradient overlay */
		bool colorForOverlayPresent = BadgeOverlayColor != null && BadgeOverlayColor != UIColor.Clear;
		if (colorForOverlayPresent) {
			ctx.SaveState();
			{
				ctx.AddPath(borderPath.CGPath);
				ctx.Clip();

				nfloat height = rectToDraw.Height;
				nfloat width = rectToDraw.Width;

				CGRect rectForOverlayCircle = new CGRect(rectToDraw.X,
					rectToDraw.Y - Math.Ceiling(height * 0.5),
					width,
					height);

				ctx.AddEllipseInRect(rectForOverlayCircle);
				ctx.SetFillColor(BadgeOverlayColor.CGColor);
				ctx.DrawPath(CGPathDrawingMode.Fill);
			}
			ctx.RestoreState();
		}

		/* Stroke */
		ctx.SaveState();
		{
			ctx.AddPath(borderPath.CGPath);
			ctx.SetLineWidth(BadgeStrokeWidth);
			ctx.SetStrokeColor(BadgeStrokeColor.CGColor);
			ctx.DrawPath(CGPathDrawingMode.Stroke);
		}
		ctx.RestoreState();

		/* Text */
		ctx.SaveState();
		{
			ctx.SetFillColor(BadgeTextColor.CGColor);
			ctx.SetShadow(BadgeTextShadowOffset, (nfloat) 1.0, BadgeTextShadowColor.CGColor);

			CGRect textFrame = rectToDraw;
			CGSize textSize = SizeOfTextForCurrentSettings();

			textFrame.Height = textSize.Height;
			textFrame.Y = rectToDraw.Y + (nfloat) Math.Ceiling((rectToDraw.Height - textFrame.Height) / 2.0f);
			
			BadgeText.DrawString(
				rect: textFrame,
				font: BadgeTextFont,
				mode: UILineBreakMode.Clip,
				alignment: UITextAlignment.Center);

		}
		ctx.RestoreState();

	}

}


