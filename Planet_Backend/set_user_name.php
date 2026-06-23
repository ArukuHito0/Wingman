<?php
require_once 'db_connect.php';
require_once 'user_resolver.php';
require_once 'user.php';

$userId = $_POST['user_id'];
$userName = $_POST['user_name'];   // Unityで設定したユーザー名

$userResolver = new UserResolver($db);
$id = $userResolver->resolve_id($userId);    // 渡されたユーザーIDのDBのIDを受け取る
$user = new User($db, $id);
$user->update_user_name($userName);  // ユーザー名を更新
?>