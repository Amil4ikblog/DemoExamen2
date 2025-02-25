-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1:3306
-- Время создания: Фев 25 2025 г., 14:02
-- Версия сервера: 8.0.19
-- Версия PHP: 7.1.33

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `auto_technical_center`
--

-- --------------------------------------------------------

--
-- Структура таблицы `auto`
--

CREATE TABLE `auto` (
  `Id` int NOT NULL,
  `Marka` varchar(100) NOT NULL,
  `model` varchar(100) NOT NULL,
  `year_of_release` date NOT NULL,
  `State_registration_number` varchar(100) NOT NULL,
  `Status` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `auto`
--

INSERT INTO `auto` (`Id`, `Marka`, `model`, `year_of_release`, `State_registration_number`, `Status`) VALUES
(101, 'Hyundai', 'Elantra', '2022-02-11', 'G789JK', 1),
(102, 'Chevrolet', 'Malibu', '2021-02-27', 'F678IJ', 0),
(103, 'Mazda', 'CX-5', '2023-02-04', 'H890KL', 1),
(104, 'Nissan', 'Altima', '2020-02-19', 'E567GH', 0);

-- --------------------------------------------------------

--
-- Структура таблицы `clients`
--

CREATE TABLE `clients` (
  `Id` int NOT NULL,
  `ID_auto` int NOT NULL,
  `FIO` varchar(100) NOT NULL,
  `contact_phone_numbers` varchar(12) NOT NULL,
  `email_addresses` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `clients`
--

INSERT INTO `clients` (`Id`, `ID_auto`, `FIO`, `contact_phone_numbers`, `email_addresses`) VALUES
(1, 101, 'Иванов Иван Иванович', '123-456-7890', 'ivanov@example.com'),
(2, 102, 'Петров Петр Петрович', '234-567-8901', 'petrov@example.com'),
(3, 103, 'Сидоров Сидор Сидорович', '345-678-9012', 'sidorov@example.com'),
(4, 104, 'Кузнецов Николай Николаевич', '456-789-0123', 'kuznetsov@example.com');

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `auto`
--
ALTER TABLE `auto`
  ADD PRIMARY KEY (`Id`);

--
-- Индексы таблицы `clients`
--
ALTER TABLE `clients`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `ID_auto` (`ID_auto`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `auto`
--
ALTER TABLE `auto`
  MODIFY `Id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=105;

--
-- AUTO_INCREMENT для таблицы `clients`
--
ALTER TABLE `clients`
  MODIFY `Id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `auto`
--
ALTER TABLE `auto`
  ADD CONSTRAINT `auto_ibfk_1` FOREIGN KEY (`Id`) REFERENCES `clients` (`ID_auto`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
