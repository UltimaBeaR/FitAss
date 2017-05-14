"use strict"
{
    // Требуется для формирования полного output пути
    let path = require('path');

    // Плагин для очистки выходной папки (bundles) перед созданием новой
    const CleanWebpackPlugin = require('clean-webpack-plugin');

    // Путь к выходной папке
    const bundlesFolder = "wwwroot/bundles/";

    module.exports = {
        // Точка входа в приложение
        entry: "./Scripts/main.ts",

        // Выходной файл
        output: {
            filename: 'bundle.js',
            path: path.resolve(__dirname, bundlesFolder)
        },

        module: {
            rules: [
                {
                    test: /\.tsx?$/,
                    loader: "ts-loader",
                    exclude: /node_modules/,
                },
            ]
        },
        resolve: {
            extensions: [".tsx", ".ts", ".js"]
        },
        plugins: [
            new CleanWebpackPlugin([bundlesFolder])
        ],

        // Включаем генерацию отладочной информации внутри выходного файла (Нужно для работы отладки клиентских скриптов)
        devtool: "inline-source-map"
    };
}