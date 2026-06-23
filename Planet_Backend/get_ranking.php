<?php
require_once 'db_connect.php';
require_once 'user.php';

$stmt = $db->prepare("SELECT id, user_name, user_icon, best_score FROM users WHERE best_score > 0 ORDER BY best_score DESC");
$stmt->execute();
$ranking = $stmt->fetchAll(PDO::FETCH_ASSOC);

echo json_encode($ranking);
?>