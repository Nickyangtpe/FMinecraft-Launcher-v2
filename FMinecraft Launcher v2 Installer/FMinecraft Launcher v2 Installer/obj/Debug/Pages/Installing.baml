   M S B A M L   `   `   `  ����[  WFMinecraft Launcher v2 Installer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:FMinecraft_Launcher_v2_Installer FMinecraft_Launcher_v2_Installer  5   	   7    1FMinecraft_Launcher_v2_Installer.Pages.Installing   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @local.clr-namespace:FMinecraft_Launcher_v2_Installer     5   	     N� Title$#  FMinecraft Launcher Installer��5   	   $��367.6��6/   $	��604��6>    N� 
Background
 ����  � 	Resources 5
         
FlatButton �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers	 P� Property
 P� Value��%�  &	        �� 5   
   " ��6#   .�5      ��  � �� �#2E7D326+   �� 5       � ��
 �����6+   �� 5       � ��$ 0q�60   �� 5       � ��$
 15,5q�6(   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   $ 5��6K   ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5       5      P� 5      	 �@J�IsMouseOver
 . . �5      ��  � �� �#1B5E2063   5      5      5      .��5!      �� $ 10��6   ���#1E1E1E._�5"   
   �  � RowDefinitions 5#      �� 5$      $��30 �6    �� 5%      $��* �6    �� 5&      $
��Auto �6    .+�5*      � -   5        ���#2E7D32.+�5+      �� $#��FMinecraft Launcher Installer��6   
�������$
��Bold"�6T   $��10,0,0,0q�6f   $��Center=�6x   �� 5,      -   5        $��X��5,      $��Right=�6%   $��30��6A   
������ $��0q�6e   
�������6�   5-      � 50      $��1��6   $��20q�6     � ColumnDefinitions 51      �� 52      $��1* �6'   �� 53      $��2* �6'   .+�57      �� $��0��6   $��0,0,20,0q�6-   .+�58      �� $��Installation Steps��6    ���#E0E0E0$
��Bold"�6O   $��18(�6a   $��0,0,0,10q�6o   �� 59      $��
1. Welcome��6    ���#B0B0B0$	��0,5q�6G   �� 5:      $��2. License Agreement��6    ���	#FFB0B0B0$	��0,5q�6S   �� 5;      $��3. Installing��6    ���	#FFD1D1D1$	��0,5q�6L   �� 5<      $��	4. Finish��6    ���#B0B0B0$	��0,5q�6F   5=      � 5@      $��1��6    5A      �� 5B      $
��Auto �6(   �� 5C      $��* �6(   �� 5D      $
��Auto �6(   .+�5G      �� $$��Installing FMinecraft Launcher��6    ���#E0E0E0$
��Bold"�6[   $��23(�6m   $��0,0,0,20q�6{   � 5I      -   5         �Name$ ProgressBar��5I   "   $��2��67   $��10��6D    � Value$ 0Q�6P   $��	0,20,0,10q�6Z   ���#4CAF50���#4242426�   �� 5J      -   5         ��Name$ ProgressLabel��5J      $#��Getting the latest version...��63   $
��Left=�6[   $��-5,139.4,0,17.8q�6v   $��1��6�   ���	#FFE0E0E0$	��381��6�   $��2��6�   5K      5L      �� 5O      $��2��6    �� Orientation$ 
Horizontal=�6&   $��Right=�6?   $��20q�6[   .+�5P      �� -   5         ��Name$ 
NextButton��5P      $
��Next��6-   #��[  6<   $��	0,0,0,0.2q�6`   $	��120��6s   $��25��6   {�.  6�   5Q      5R      5S      5T      