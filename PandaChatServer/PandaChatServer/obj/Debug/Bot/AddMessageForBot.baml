   M S B A M L   `   `   `  ����J  FPandaChatServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:PandaChatServerPandaChatServer  5   	   &     PandaChatServer.AddMessageForBot   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:PandaChatServer     5   	     � Title$5  /Редактирование сообщений��5   	   $��305.167��6*   $��445.5��6;   ���	#FF1D1D1D
�������.��5	      � .+�5
   
   �� -   5         ��Name$ MessageOfBot��5
      $
��Left=�6(   $	��100��6C   $��	10,88,0,0q�6P    �� TextWrapping$
 Wrap=�6c   $	��Top=�6w   $	��418��6�   ���	#FF1D1D1D
�������$��Consolas)�6�   $
��Bold"�6�    �� AcceptsReturn . 6�   �� 5   
   -   5        $ NameOfMessage��5      $
��Left=�6)   $��24��6D   $��	10,39,0,0q�6P   $
 Wrap=�6c   $	��Top=�6w   $	��418��6�   ���	#FF1D1D1D
�������$��Consolas)�6�   $
��Bold"�6�   �� 5   
   -   5         ��Name$ label��5      $��Название:��6   $
��Left=�63   $��	10,10,0,0q�6N   $	��Top=�6a   
�������$��Consolas)�6�   $
��Bold"�6�   �� 5   
   -   5        $ label1��5      $��Сообщение:��6    $
��Left=�65   $��	10,63,0,0q�6P   $	��Top=�6c   
�������$��Consolas)�6�   $
��Bold"�6�   �� 5   
   -   5         ��Name$ SaveBotMessage��5      $��Сохранить��6)   $��10,237,10,0q�6=   ���	#FF25934F
�������$��Consolas)�6|   $��20��6�   $	��Top=�6�   6�   �� 5   
   -   5         ��Name$ BotMessageType��5      $
��Left=�6+   $��164,202,0,0q�6F   $	��Top=�6[   $	��264��6s    �� SelectedIndex$ 0��6   $��Consolas)�6�   $
��Bold"�6�   ��5      �� 5       �� EndPoint$	 0,1&�6&   	 �� 
StartPoint$		 0,0&�65   .��5      � 
 � Color$
 	#FFF0F0F0��6#    � Offset$ 0Q�65   � 5      $
 	#FF1F1F1F��6#   $ 1Q�65   5      .@�5      �� $/��)Рассылаемое сообщение��6   �� 5      $��Черный список��6   5      �� 5   
   $-��'Тип нового сообщения:��6   $
��Left=�60   $��
10,200,0,0q�6K   $	��Top=�6_   $��Consolas)�6w   $
��Bold"�6�   
�������6�   5      5      