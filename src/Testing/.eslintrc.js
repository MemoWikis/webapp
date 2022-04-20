const config = {
    extends: ['plugin:cypress/recommended', 'prettier', 'plugin:@typescript-eslint/recommended'],
    parser: '@typescript-eslint/parser',
    plugins: ['cypress', '@typescript-eslint'],
    rules: {
        '@typescript-eslint/explicit-module-boundary-types': 'off',
        '@typescript-eslint/no-empty-function': 'off',
        '@typescript-eslint/no-unused-vars': [
            'error',
            {
                vars: 'all',
                args: 'after-used',
                argsIgnorePattern: '^_',
                varsIgnorePattern: '^_',
                ignoreRestSiblings: false
            },
        ],
        '@typescript-eslint/no-non-null-assertion': 'off',
        '@typescript-eslint/ban-ts-comment': 'off',
        '@typescript-eslint/no-inferrable-types': 'off',
    },
};

module.exports = config;
