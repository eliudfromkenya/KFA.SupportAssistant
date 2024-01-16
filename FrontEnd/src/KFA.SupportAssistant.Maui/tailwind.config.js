/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./**/*.{html,cshtml,razor,js}"],
    theme: {
        extend: {
            fontSize: {
                xs: "0.75em",
                sm: "1em",
                md: "1.25em",
                base: "1.5em",
                lg: "1.75em",
                xl: "2em",
            },
            fontFamily: {
                mukta: ["Mukta", "sans-serif"],
            },
            colors: {
                transparent: "transparent",
                current: "currentColor",
                customBlack: {
                    one: "#141414",
                    two: "#333333",
                    three: "#4D4D4D",
                },
                gray: {
                    one: "#666666",
                    two: "#808080",
                    three: "#999999",
                },
                lightGray: {
                    one: "#B3B3B3",
                    two: "#CCCCCC",
                    three: "#E6E6E6",
                    four: "#FFF2F2F2",
                },
                blue: {
                    one: "#012A4A",
                    two: "#013A63",
                    three: "#01497C",
                    four: "#014F86",
                },
                lightBlue: {
                    one: "#2A6F97",
                    two: "#2C7DA0",
                    three: "#468FAF",
                    four: "#61A5C2",
                },
                lighterBlue: {
                    one: "#89C2D9",
                    two: "#B5E2FF",
                    three: "#DAF0FF"
                },
                red: {
                    one: "#660007",
                    two: "#99000A",
                    three: "#CC000E",
                    four: "#FF0011",
                },
                lightRed: {
                    one: "#FF5964",
                    two: "#FF99A0",
                    three: "#FFCCCF",
                    four: "#FFE6E7",
                },
                wine: {
                    one: "#461220",
                    two: "#7A1F38",
                    three: "#A22A4A",
                    four: "#CB345D",
                },
                pink: {
                    one: "#D55D7D",
                    two: "#E0859E",
                    three: "#EAAEBE",
                    four: "#F5D6DF",
                    five: "#FAEBEF",
                },
            },
            width: {
                w: {
                    50: "200px",
                },
            },
            height: {
                h: {
                    50: "200px",
                    100: "700px"
                },
            },
        },
    },
    plugins: {
        tailwindcss: {},
        autoprefixer: {},
    }
}

