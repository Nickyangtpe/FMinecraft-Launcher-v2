   M S B A M L   `   `   `  ����   FMinecraft Launcher v20    *FMinecraft_Launcher_v2.Windows.ErrorWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  FMinecraft Launcher��$	��250��$	��450�� � WindowStartupLocation$ CenterScreen=����	#FF2A2A2A � 
ResizeMode$ NoResize=�$��Segoe UI)� � 	Resources     ComboBoxToggleButtonTemplate �� 
TargetType � ColumnDefinitions �� CornerRadius �� Triggers P� Property	 P� Value
 �� 
TargetName �� Property �� Value  ComboBoxPopupBackground �� Color  ComboBoxPopupBorder �� 
TargetType  VersionComboBoxStyle d� 	IsChecked �� Mode �� RelativeSource J� IsHitTestVisible �� SelectionBoxItem �� SelectionBoxItemTemplate � AllowsTransparency � PlacementTarget �� ElementName � ActualWidth �� Padding  NavigationBarButtonStyle �� Triggers  ErrorButtonStyle �� BasedOn��%�  &	        &	 P    &	 e    &	 z    7   7  7  &	 �    &	 �    7���� " d�.R��  �� �� $��20 �.+��� $��Border��$��2��#��z��#��z��#��z��$ 2��F� $��1��"����        � 	=  	= � z     	$��Center=�$��Center=�#��z�� P�  �@J�IsMouseOver$
	 True��. ��� $
 Border�� � �� �	#FF444444P�  � ��$
	 True��. ��� $
 Border�� � �� �	#FF555555�� $ 	#FF333333���� $ 	#FF555555���� " ��.���  � �� �	#FF333333��  � ��
 �������  � �� �	#FF555555��  � ��$ 1q���  � ��$ 5,0,0,0q���  � �� �� " ��.R�� .+�d� $��ToggleButton��8��   �� *IsDropDownOpen+$ TwoWay=� �� *TemplatedParent+�� $��ContentSite��$ False��#��z #��z #��z��$
��Left=�#��z��� $��Popup��$
 True��$|�False��$��Slide=� �� $ ToggleButton������ *IsDropDownOpen+$ TwoWay=� �� *TemplatedParent+.��� #��z $	��300��.+��� 8�� 8�� $��1q��� $
��Auto=�.���� ��  � �� �� " ��.���  � ��
 ���� ��  � ��
 �������  � ��$ 5q���  � �� �� " ��.R��� #��z��# z��._��� $
��Left=�$��Center=� P�  �@��IsHighlighted$
	 True��. ���  � �� �	#FF555555�� " ��.���  � ��
 ���� ��  � ��
 �������  � ��$ 0q���  �@�Cursor$
 Hand����  � ��$ 0,0,20,0q���  � �� �� " ��.R��� #��z��#��z������ " ��.���  � ��$
 None{� P�  �@J�IsMouseOver$
	 True��. ���  � ��$ 	Underline{��� " ��8   .���  � �� �	#FFD43C3C��  � ��
 �������  � ��$ 0q���  � ��$
 15,5q���  � ��$ 5q���  � ��$
 Bold"���  � �� �� " ��.R��� #��z��#��z��#��z��$ 5��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
	 True��. ���  � �� �	#FFB02D2DP�  � ��$
	 True��. ���  � �� �	#FF972424.��� $��20,20,20,13q� � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0�� �� Orientation$ 
Horizontal=�$��Center=�$��0,0,0,15q�.+��� $	��❗��$��48(�$
��Bold"�
�������$��Center=�$��0,0,10,0q��� $��An Error Occurred��$��24(�$
��Bold"�
�������$��Center=��� -    ��Name$ 	ErrorText��$��1��$��
Error Text��
�������$
��Wrap=�$��Center=�$��14(�$	��100��$��WordEllipsis=��� $��2��$ 
Horizontal=�$��Right=�$��0,15,0,0q�.+��� -   $��Close��#��[ $	��124��$��28��