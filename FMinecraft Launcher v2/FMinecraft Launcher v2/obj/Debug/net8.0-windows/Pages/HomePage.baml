   M S B A M L   `   `   `  ����  FMinecraft Launcher v2+    %FMinecraft_Launcher_v2.Pages.HomePage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     N� Title$  FMinecraft Launcher��5   	   $	��487��6%   $	��790��62    N� 
Background �	#FF2A2A2A � 	Resources 5          ComboBoxToggleButtonTemplate �� 
TargetType � ColumnDefinitions �� CornerRadius �� Triggers P� Property P� Value	 �� 
TargetName
 �� Property �� Value  ComboBoxPopupBackground �� Color  ComboBoxPopupBorder �� 
TargetType  VersionComboBoxStyle d� 	IsChecked �� Mode �� RelativeSource J� IsHitTestVisible �� SelectionBoxItem �� TextAlignment � AllowsTransparency � PlacementTarget �� ElementName � ActualWidth �� Padding  NavigationBarButtonStyle �� Triggers " NavigationBarButtonStyle_First �� BasedOn ! NavigationBarButtonStyle_Last  ProgressBarStyle �Name ��Name J� Effect3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   
BlurRadius    Color!   Opacity" :� RoutedEvent# �� 
TargetName$ �� TargetProperty% g� Duration& W� EasingFunction.  (System.Windows.Media.Animation.CubicEase7  1System.Windows.Media.Animation.EasingFunctionBase'   
EasingMode��%p  &	        &	 Z    &	 }    &	 �    7   7  7  &	 �    &	 �
    7  &	 +    7  &	 }    �� 5   
   " d�6?   .R�5	      �  5
      �� 5      �� 5      $��20 �6'   .+�5      �� $��Border��6   $��2��5      #��z��5      #��z��5      #��z��5      $ 2��5      F� 5      $��1��6   "����        � 	=  	= � z     	$��Center=�5      $��Center=�5      #��z��5      5       5      P� 5       �@J�IsMouseOver$
 True��61   . �5      �� $	 Border��6   
 � �� �	#FF4444446G   5      P� 5       � ��$
 True��6/   . �5      �� $	 Border��6   
 � �� �	#FF5555556G   5       5"      �� 5%   
   $ 	#FF333333��6:   �� 5&   
   $ 	#FF555555��66   �� 5(   
   " ��6   .�5)      �� 
 � �� �	#FF3333336+   �� 5*      
 � ��
 �����6+   �� 5+      
 � �� �	#FF5555556,   �� 5,      
 � ��$ 1q�60   �� 5-      
 � ��$ 5,0,0,0q�6(   �� 5.      
 � �� 5/      �� 50      " ��6&   .R�51      � .+�52      d� $��ToggleButton��6+   8��   54   #   �� *IsDropDownOpen+$ TwoWay=� �� *TemplatedParent+�� 55      $��ContentSite��6/   $ False��56   '   #��z 57   '   #��z��58   '   $
��Left=�59   '   $��0,0,10,0q�5:   '   ��5;   "   �� 5<   &   .R�5=   *   �� $��28(�6E   $
 Left=�6S   ��64   �� 6S   5>   '   5@      � 5A      $��Popup��6$   $
 True��5D      $|�False��5E      $��Slide=�5F       5B      �� $ ToggleButton����5C      �� *IsDropDownOpen+$ TwoWay=� �� *TemplatedParent+.��5G   "   � #��z 6'   $	��300��6P   .+�5H   &   �� 8�� 8�� $��1q�6�   �� 5I   &   $
��Auto=�63   .��5J   *   �� 5K   '   5L   #   5M      5N      5O      5Q      �� 5R      
 � �� 5S      �� 5T      " ��6   .�5U      �� 
 � ��
 ���� 67   �� 5V      
 � ��
 �����67   �� 5W      
 � ��$ 5q�64   �� 5X      
 � �� 5Y      �� 5Z   "   " ��62   .R�5[   &   �� #��z��6-   # z��6W   ._�5\   *   �� 5]   '    5^   &   P� 5_   *    �@��IsHighlighted$
 True��6K   . �5`   .   �� 
 � �� �	#FF5555556K   5a   +   5c   #   5e      5f      5h      5i      �� 5l   
   " ��6   .�5m      �� 
 � ��
 ���� 6+   �� 5n      
 � ��
 �����6+   �� 5o      
 � ��$ 0q�60   �� 5p      
 �@�Cursor$
 Hand��6'   �� 5q      
 � ��$ 0,0,20,0q�6'   �� 5r      
 � �� 5s      �� 5t      " ��6&   .R�5u      �� #��z��6$   #��z��5v   $   ��5w      �� 5x   "   " ��6(   .�5y   &   �� 
 � ��$
 None{�6H    5z   &   P� 5{   *    �@J�IsMouseOver$
 True��6I   . �5|   .   �� 
 � ��$ 	Underline{�6P   5}   +   5   #   5�      5�      5�      5�      �� 5�   
   " ��6   8   .�5�      �� 
 � ��$ 0,0,20,0q�6'   5�      �� 5�   
   " ��6   8   .�5�      �� 
 � ��$ 0,0,0,0q�6'   5�      �� 5�   
   " �6)   .�5�      �� 
 � �� �	#FF3333336+   �� 5�      
 � �� �	#FF3A8E4C6+   �� 5�      
 � ��$ 6��6'   �� 5�      
 � �� 5�      �� 5�      " �6&   .R�5�      � $ TemplateRoot��6   .+�5�      �� #��z��6%   $ 3��5�   !   ��5�   !   Z� �� 5�      $ 
PART_Track��6%   #��z��5�   !   $ 3��5�   !   �� 5�      $ PART_Indicator��6%   #��z��5�   !   $ 3��5�   !   $
��Left=�5�   !   $��0,0,0,0q�5�   !    5�   "     5�   &   $ 0Q�67   $ 4Q�5�   1   $  	#FF3A8E4C��5�   1   $	! 0.3Q�5�   1   5�      5�       5�      :� 5�      $" ProgressBar.ValueChanged��6+   .[�5�   "   �� .��5�   &   �� .��5�   *   W� $# PART_Indicator��6:   $$ Width�5�   4   $% 0:0:0.2D�5�   4   & 5�   .     5�   2   $' EaseOut=�6<   5�   +   5�   '   5�   #   5�      5�      5�      5�      5�      .��5�      � ( � RowDefinitions( 5�   
   �� 5�      $
��Auto �6   �� 5�      $��* �6   �� 5�      $
��Auto �6   .+�5�   
   �� $��0��6   ���	#FF1E1E1E._�5�      �� ) �� Orientation$) 
Horizontal=�6   $��20,10q�62   .+�5�      �� $
��Play��6   #��[ 6(   �� 5�      -   5        $��Settings��5�      
�������#��[ 6>   6p   �� 5�      -   5        $��About us��5�      
�������#��[ 6>   6p   �� 5�      -   5        $��GitHub��5�      
�������#��[ 6<   6s   5�      5�      � 5�   
   $��1��6   ( 5�      �� 5�      �� 5�      $
��Auto �6    $��80��6.   �� 5�      $
��Auto �6    .+�5�      �� -   5        * ��Name$* Cover��5�      $��0��6#   $��20,20,20,79q�60   $��2��6E   $-��'/Pages/1.17_caves_and_cliffs_part_1.jpg��6V   $
��Fill=�6�   � 5�      $��1��6   $��20q�6     5�      �� 5�      $
��Auto �6'   �� 5�      .+�5�      �� -   5        + ��Name$+ VerisonComboBox��5�      $��0��64   #��[ 6D   $��2��6r   $��	0,2,443,2q�6�   $��Bahnschrift)�6�   ���	#FF2A2A2A$��20(�6�   �� 5�      -   5        , ��Name$, LaunchButton��5�      $��1��6/   ���	#FF3A8E4C
�������$��16(�6i   $
��Bold"�6w   $��Right=�6�   $	��282��6�   ��6�   Z� ��5�      �� 5�      " ��6    # [B��6=   .�5�      �� 
 � �� 5�   "   �� 5�   &   " ��66   .R�5�   *   �� #��z��61   #��z��5�   1   #��z��5�   1   $ 10��5�   1   ._�5�   .   �� $��Center=�6?   $��Center=�6\   5�   +   5�   '   5�      5�      .��5�      �� $��18(�6    $��Yu Gothic UI Semibold)�6.   $��Launch��6Q   5�      5�      � 5�      -   5        - �Name$- DownloadProgressBar��5�   Q   $��1��6   #��[ 6'   . � IsIndeterminate$. False��6n   $��
20,70,20,6q�6�   / � Value$/ 0Q�6�   $��8��6�   ���	#FF1F1F1F��6�   Z� 6�   5�      5�      5�      