using CommunityToolkit.Maui.Animations;
using CommunityToolkit.Maui.Extensions;

using uom.maui;

namespace uom.controls.MAUI.Animations
{

	internal static class Helpers
	{

		public static async Task WaitForButtonAnimation(Int32? delay = 800)
		{
			try
			{
				delay ??= "ButtonClickDelay".FindAppResource_Int32(100);
				//string s = $"Type: '{d.GetType()}', value: '{d}'";
				await Task.Delay(delay.Value); //Add delay for button click animation				
			}
			catch { }
		}

		public static async Task WaitForButtonAnimation(this Button btn, Int32? delay = 800) => await WaitForButtonAnimation();
	}


	class PaintTheRainbowAnimation : BaseAnimation
	{
		public override async Task Animate(VisualElement view, CancellationToken token = default)
		{
			await view.BackgroundColorTo(Colors.Red);
			await view.BackgroundColorTo(Colors.Orange);
			await view.BackgroundColorTo(Colors.Yellow);
			await view.BackgroundColorTo(Colors.Green);
			await view.BackgroundColorTo(Colors.Blue);
			await view.BackgroundColorTo(Colors.Indigo);
			await view.BackgroundColorTo(Colors.Violet);
		}
	}


	class SampleScaleAnimation : BaseAnimation
	{
		public override async Task Animate(VisualElement view, CancellationToken token)
		{
			await view.ScaleTo(1.2, Length, Easing).WaitAsync(token);
			await view.ScaleTo(1, Length, Easing).WaitAsync(token);
		}
	}

	class SampleScaleToAnimation : BaseAnimation
	{
		public double Scale { get; set; }

		public override Task Animate(VisualElement view, CancellationToken token)
			=> view.ScaleTo(Scale, Length, Easing).WaitAsync(token);
	}


	public class Fade2Animation : BaseAnimation
	{

		public static readonly BindableProperty OpacityFromProperty =
			BindableProperty.Create(
				nameof(OpacityFrom),
				typeof(double),
				typeof(Fade2Animation),
				0.0,
				BindingMode.TwoWay);

		public static readonly BindableProperty OpacityToProperty =
			BindableProperty.Create(
				nameof(OpacityTo),
				typeof(double),
				typeof(Fade2Animation),
				1.0,
				BindingMode.TwoWay);


		public Fade2Animation() : base(300) { }



		/// <summary>Gets or sets the opacity to fade from.</summary>
		public double OpacityFrom
		{
			get => (double)GetValue(OpacityFromProperty);
			set => SetValue(OpacityFromProperty, value);
		}

		/// <summary>Gets or sets the opacity to fade to </summary>
		public double OpacityTo
		{
			get => (double)GetValue(OpacityToProperty);
			set => SetValue(OpacityToProperty, value);
		}



		/// <inheritdoc />
		public override async Task Animate(VisualElement view, CancellationToken token = default)
		{
			ArgumentNullException.ThrowIfNull(view);

			//var originalOpacity = view.Opacity;
			//await view.FadeTo(Opacity, Length, Easing).WaitAsync(token);
			view.Opacity = OpacityFrom;
			await view.FadeTo(OpacityTo, Length, Easing).WaitAsync(token);
		}
	}


	public class FadeInAnimation : Fade2Animation
	{
		public FadeInAnimation() : base()
		{
			OpacityFrom = 0.0;
			OpacityTo = 1.0;
			this.Length = 100;
		}
	}


	public class FadeFlashAnimation : BaseAnimation
	{

		public FadeFlashAnimation() : base(300u) { }

		public override async Task Animate(VisualElement view, CancellationToken token = default(CancellationToken))
		{
			ArgumentNullException.ThrowIfNull(view, nameof(view));

			using CancellationTokenSource cts = new();
			if (token == null) token = cts.Token; ;

			void stopAni(object? s, EventArgs e) => cts.Cancel();

			try
			{

				view.BindingContextChanged += stopAni;

				if (view.IsVisible && !token.IsCancellationRequested) await view.FadeTo(0f, base.Length, base.Easing).WaitAsync(token);
				if (view.IsVisible && !token.IsCancellationRequested) await view.FadeTo(1f, base.Length, base.Easing).WaitAsync(token);
			}
			catch { }
			finally
			{
				view.Opacity = 1f;
				view.BindingContextChanged -= stopAni;
			}
		}
	}

}
