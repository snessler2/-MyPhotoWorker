   M S B A M L   `   `   `  ����H  DMyPhotoWorker, Version=0.9.1.1, Culture=neutral, PublicKeyToken=null-clr-namespace:MyPhotoWorkerMyPhotoWorker  5   	   !    MyPhotoWorker.AddInfoDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:MyPhotoWorker     5   	   -   5          � Title$  Add Info Text��5   	    � SizeToContent$ WidthAndHeight=�6    � WindowStartupLocation$ CenterScreen=�6>   .��5
      � $��15q�6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� -   5        $��
lblInfoAdd��5      $��1��6"   .��62   Zu addierender Infotext:6L   �� 5   
   -   5        $��
txtInfoAdd��5      $��1��6$   $��1��64   $	��250��6A   6R   �� 5   
   -   5        $��lboxInfoAdd��5      $��1��6%   $��2��65   $	��100��6B   ��5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6!   .+�5      �� ��6$   �� 5      5       5"      � 5%   
   $��3��6   $��2��6!   $��Right=�65   $��0,15,0,0q�6Q   .+�5&      �� -   5        $��btnDialogOk��5&   &    �� 	IsDefault . $��60��6S   $��0,0,10,0q�6a   .��6s   _Ok6x   �� 5'       �� IsCancel . $��60��6%   .��63   	_Cancel6<   5(      5)      5+      