   M S B A M L   `   `   `  ����   FMinecraft Launcher v2+    %FMinecraft_Launcher_v2.Pages.HomePage   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       N� Title$  FMinecraft Launcher��$	��487��$	��790�� N� 
Background �	#FF2A2A2A � 	Resources     ComboBoxToggleButtonTemplate �� 
TargetType � ColumnDefinitions �� CornerRadius �� Triggers P� Property P� Value	 �� 
TargetName
 �� Property �� Value  ComboBoxPopupBackground �� Color  ComboBoxPopupBorder �� 
TargetType  VersionComboBoxStyle d� 	IsChecked �� Mode �� RelativeSource J� IsHitTestVisible �� SelectionBoxItem �� TextAlignment � AllowsTransparency � PlacementTarget �� ElementName � ActualWidth �� Padding  NavigationBarButtonStyle �� Triggers " NavigationBarButtonStyle_First �� BasedOn ! NavigationBarButtonStyle_Last  ProgressBarStyle �Name ��Name J� Effect3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   
BlurRadius    Color!   Opacity" :� RoutedEvent# �� 
TargetName$ �� TargetProperty% g� Duration& W� EasingFunction.  (System.Windows.Media.Animation.CubicEase7  1System.Windows.Media.Animation.EasingFunctionBase'   
EasingMode��%�  &	        &	 P    &	 e    &	 z    7   7  7  &	 �    &	     7  &	 2    7  &	 _    �� " d�.R��  �� �� $��20 �.+��� $��Border��$��2��#��z��#��z��#��z��$ 2��F� $��1��"����        � 	=  	= � z     	$��Center=�$��Center=�#��z�� P�  �@J�IsMouseOver$
 True��. ��� $	 Border��
 � �� �	#FF444444P�  � ��$
 True��. ��� $	 Border��
 � �� �	#FF555555�� $ 	#FF333333���� $ 	#FF555555���� " ��.��� 
 � �� �	#FF333333�� 
 � ��
 ������� 
 � �� �	#FF555555�� 
 � ��$ 1q��� 
 � ��$ 5,0,0,0q��� 
 � �� �� " ��.R�� .+�d� $��ToggleButton��8��   �� *IsDropDownOpen+$ TwoWay=� �� *TemplatedParent+�� $��ContentSite��$ False��#��z #��z��$
��Left=�$��0,0,10,0q����� .R��� $��28(�$
 Left=����� � $��Popup��$
 True��$|�False��$��Slide=� �� $ ToggleButton������ *IsDropDownOpen+$ TwoWay=� �� *TemplatedParent+.��� #��z $	��300��.+��� 8�� 8�� $��1q��� $
��Auto=�.���� �� 
 � �� �� " ��.��� 
 � ��
 ���� �� 
 � ��
 ������� 
 � ��$ 5q��� 
 � �� �� " ��.R��� #��z��# z��._���  P�  �@��IsHighlighted$
 True��. ��� 
 � �� �	#FF555555�� " ��.��� 
 � ��
 ���� �� 
 � ��
 ������� 
 � ��$ 0q��� 
 �@�Cursor$
 Hand���� 
 � ��$ 0,0,20,0q��� 
 � �� �� " ��.R��� #��z��#��z������ " ��.��� 
 � ��$
 None{� P�  �@J�IsMouseOver$
 True��. ��� 
 � ��$ 	Underline{��� " ��8   .��� 
 � ��$ 0,0,20,0q��� " ��8   .��� 
 � ��$ 0,0,0,0q��� " �.��� 
 � �� �	#FF333333�� 
 � �� �	#FF3A8E4C�� 
 � ��$ 6���� 
 � �� �� " �.R�� $ TemplateRoot��.+��� #��z��$ 3����Z� �� $ 
PART_Track��#��z��$ 3���� $ PART_Indicator��#��z��$ 3��$
��Left=�$��0,0,0,0q�   $ 0Q�$ 4Q�$  	#FF3A8E4C��$	! 0.3Q� :� $" ProgressBar.ValueChanged��.[��� .���� .��W� $# PART_Indicator��$$ Width�$% 0:0:0.2D�&   $' EaseOut=�.��� ( � RowDefinitions( �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0�����	#FF1E1E1E._��� ) �� Orientation$) 
Horizontal=�$��20,10q�.+��� $
��Play��#��[ �� -   $��Settings��
�������#��[ �� -   $��About us��
�������#��[ �� -   $��GitHub��
�������#��[ � $��1��( �� �� $
��Auto �$��80���� $
��Auto �.+��� -   * ��Name$* Cover��$��0��$��20,20,20,79q�$��2��$-��'/Pages/1.17_caves_and_cliffs_part_1.jpg��$
��Fill=�� $��1��$��20q� �� $
��Auto ��� .+��� -   + ��Name$+ VerisonComboBox��$��0��#��[ $��2��$��	0,2,443,2q�$��Bahnschrift)����	#FF2A2A2A$��20(��� -   , ��Name$, LaunchButton��$��1�����	#FF3A8E4C
�������$��16(�$
��Bold"�$��Right=�$	��282����Z� ���� " ��# [B��.��� 
 � �� �� " ��.R��� #��z��#��z��#��z��$ 10��._��� $��Center=�$��Center=�.���� $��18(�$��Yu Gothic UI Semibold)�$��Launch��� -   - �Name$- DownloadProgressBar��$��1��#��[ . � IsIndeterminate$. False��$��
20,70,20,6q�/ � Value$/ 0Q�$��8�����	#FF1F1F1F��Z� 