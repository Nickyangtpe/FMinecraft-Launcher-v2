   M S B A M L   `   `   `  ����   FMinecraft Launcher v22    ,FMinecraft_Launcher_v2.Windows.LoadingWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  FMinecraft Launcher��$	��200��$	��300�� � WindowStyle$
 None=� � AllowsTransparency$
 True��
������  � WindowStartupLocation$ CenterScreen=� � Topmost$
 True��.����  �� CornerRadius$ 10�����	#FF2A2A2A���#404040$��1q�._��  � RowDefinitions �� $��* ��� $
��Auto ��� $��* �.+��� $��FMinecraft Launcher��$��20(����#00FF00$��Center=�$��Center=��� $��1��$��Center=�.+�� $	��150��$��4�����#333333.+��� ���#333333$��Stretch=�$��Center=��� $��1��$��Center=�.+�� $	��150��$��4�����#333333.+��� -    ��Name$ ProgressBar��$��30��$
��Left=����#00FF00 � Triggers :� 	 :� RoutedEvent$	 Rectangle.Loaded��.[��� .���� 
 g� RepeatBehavior$
 Forever��.��W�  �� TargetProperty$8 2(Rectangle.RenderTransform).(TranslateTransform.X)� W� From -30	 W� To 150 g� Duration$ 0:0:1D�z�T� �� -    ��Name$ LoadingText��$��Initializing...�����#CCCCCC$��0,10,0,0q�$��Center=� :� $	 TextBlock.Loaded��.[��� .���� $
 Forever��.��W� $ Opacity� 1 0.3$ 0:0:0.5D� g� AutoReverse$
 True���� -   $ 	LoadLabel��$��2��$��Downloading resources��
�������$��10(�$��Center=�$��Center=�