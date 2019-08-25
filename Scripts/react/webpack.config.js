
var path = require('path');

module.exports =
    {
        context: __dirname,
        entry: "./KanbanBoardTickets.tsx",
        output:
        {
            path: __dirname + "/dist",
            filename: "bundle.js"
        },
        watch: true,
        module:
        {
            rules:
                [
                    {
                        test: /\.js$/,
                        exclude: /(node_modules)/,
                        use:
                        {
                            loader: "babel-loader",
                            options:
                            {
                                presets: ["@babel/preset-env", "@babel/preset-react"],
                                plugins: ["@babel/plugin-proposal-class-properties"]
                            }
                        }
                    },

                    {
                        test: /\.tsx?$/,
                        exclude: /(node_modules)/,
                        use: "ts-loader"
                    }

                ],

    },

    resolve:
    {
        extensions: ['.js', '.jsx', '.tsx'],
        modules: [
            'node_modules',
            path.resolve(__dirname, 'components'),
            path.resolve(__dirname, 'contracts'),
            path.resolve(__dirname, 'enumerations'),
            path.resolve(__dirname, 'exceptions'),
            path.resolve(__dirname, 'implementations')
        ]
    }

    };



