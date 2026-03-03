module.exports = {
  preset: 'jest-preset-angular',
  setupFilesAfterFramework: ['<rootDir>/setup-jest.ts'],
  testPathPattern: ['\\.spec\\.ts$'],
  transform: { '^.+\\.(ts|js|html)$': 'jest-preset-angular' },
};