   M S B A M L   `   `   `  ����   FMinecraft Launcher v2/    )FMinecraft_Launcher_v2.Pages.SettingsPage   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Aav2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006       N� Title$  Settings��$	��500��$	��800�� N� 
Background �	#FF2A2A2A � 	Resources  �� 
TargetType   NavigationBarButtonStyle �� Property �� Value �� 
TargetType �� Triggers P� Property	 P� Value " NavigationBarButtonStyle_First
 �� BasedOn ! NavigationBarButtonStyle_Last  DarkCheckBoxStyle  DarkButtonStyle  DarkTextBoxStyle  DarkComboBoxStyle � FocusVisualStyle d� 	IsChecked �� Mode �� RelativeSource � ColumnDefinitions �� SelectionBoxItem �� SelectionBoxItemTemplate �� IsDropDownOpen � AllowsTransparency��%  &	        &	 /    7   &	 ]    7   &	 �    &	 �    &	 s    &	     �� " ��.���  � ��
 ���� ��  � ��
 �������  � ��$ 0q���  �@�Cursor$
 Hand����  � ��$ 0,0,20,0q���  � �� �� " ��.R��� #��z��#��z������ " ��.���  � ��$
 None{� P�  �@J�IsMouseOver$
	 True��. ���  � ��$ 	Underline{��� " ��8
   .���  � ��$ 0,0,20,0q��� " ��8
   .���  � ��$ 0,0,0,0q��� " ��.���  � ��
 �������  � ��$ 0,5,0,5q��� " ��.���  � �� �	#FF3A3A3A��  � ��
 �������  � ��$ 1q���  � �� �	#FF555555��  � ��$
 10,5q���  � ��$ 0,5,0,5q��� " ��.���  � �� �	#FF333333��  � ��
 �������  � �� �	#FF555555��  � ��$ 1q���  � ��$ 5q���  � ��$ 0,5,0,5q��� " ��.���  � �� �	#FF333333��  � ��
 �������  � �� �	#FF555555��  � ��$ 1q���  � ��$ 5q���  � ��$ 0,5,0,5q���  � �� �� " ��.R�� .+�d� $��ToggleButton��#��z��#��z��#��z��#��z��$��Stretch=�$��Stretch=� Z�  �� *IsDropDownOpen+$ TwoWay=� �� *TemplatedParent+.���  �� �� $��20 �.+��� $��0��#��z #��z #��z��$��Center=�$
��Left=�$��5,0,0,0q�F� $��1��"����        � 	=  	= � z     	#��z��$��Center=�$��Center=�� $��Popup��$��Bottom=�#��z $
 True��$|�False��.���� ���	#FF333333���	#FF555555$��1q�._��� $	��200��.���� .���  � RowDefinitions �� $
��Auto ��� �� $
��Auto �$��0��.+�� $��1��$��20,20,20,20q�.+��� .+��� $��Settings��
�������$��24(�$��0,0,0,20q��� -    ��Name$ HideLauncher��$��Hide Launcher on startup��#��[ �� -   $ ShowConsole��$��Show console on startup��#��[ �� -   $ CloseLauncher��$'��!Close the launcher after starting��#��[ �� -   $ TopMostLauncher��$��TopMost (Launcher)��#��[ ��  �� Orientation$ 
Horizontal=�$��0,5,0,5q�.+��� -   $ releases��$��Show releases��#��[ $��0,0,10,0q��� -   $ snapshot��$��Show snapshot��#��[ $��0,0,10,0q��� -   $ Alpha��$��
Show Alpha��#��[ $��0,0,10,0q��� $��	User Name��
�������$��0,10,0,5q��� -    ��Name$ UserName��#��[ $
��Left=�$	��345���� -	   $��Version folder��#��[ �� -
   $��Console��#��[ �� $��Reinstall Launcher��#��[ ���	#FFEB4E4E�� $��Launcher Cover��
�������$	��759���� -    ��Name$ CoverComboBox��#��[ $	��486��$��29�� �� SelectedIndex$ 11��$
��Left=�.@��� .��1.08_cats_and_pandas.jpg�� .��1.09_combat_update_java.jpg�� .��1.0_pocket_edition.jpg�� .��" 1.11_exploration_update_java.jpg�� .��%#1.12_world_of_color_update_java.jpg�� .��#!1.13_technically_updated_java.jpg�� .��1.13_update_aquatic.jpg�� .��!1.14_village_pillage_update.jpg�� .��1.15_buzzy_bees_update.jpg�� .��1.16_nether_update.jpg�� .��" 1.17_caves_and_cliffs_part_1.jpg�� .��" 1.17_caves_and_cliffs_part_2.jpg�� .��1.19_the_wild_update.jpg�� .��bedrock_and_java_together.jpg�� .��+)bedrock_and_java_together_technoblade.jpg�� .��bedrock_master.jpg�� .��bedrock_standard.jpg�� .��original_image.jpg�� .��other_dungeons.jpg�� .��other_early_console_era.jpg�� .��other_indie_days.jpg�� .��other_late_legacy_console.jpg�� .��other_mid_legacy_console.jpg�� $��0�����	#FF1E1E1E$��
0,0,-108,4q�._��� $ 
Horizontal=�$��20,10q�.+��� -   $
��Play��
�������#��[ �� $��Settings��#��[  �� -   $��About us��
�������#��[  �� -   $��GitHub��
�������#��[ 