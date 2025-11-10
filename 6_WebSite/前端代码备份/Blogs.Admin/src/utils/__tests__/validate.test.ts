import { describe, it, expect } from 'vitest'
import { validateEmail, validatePhone, validatePassword } from '@/utils/validate'

describe('Validation Utils', () => {
  describe('validateEmail', () => {
    it('should validate correct email', () => {
      expect(validateEmail('test@example.com')).toBe(true)
      expect(validateEmail('user.name@domain.co.uk')).toBe(true)
    })

    it('should reject invalid email', () => {
      expect(validateEmail('invalid-email')).toBe(false)
      expect(validateEmail('test@')).toBe(false)
      expect(validateEmail('@example.com')).toBe(false)
    })
  })

  describe('validatePhone', () => {
    it('should validate correct phone number', () => {
      expect(validatePhone('13800138000')).toBe(true)
      expect(validatePhone('15912345678')).toBe(true)
    })

    it('should reject invalid phone number', () => {
      expect(validatePhone('12345678901')).toBe(false)
      expect(validatePhone('1380013800')).toBe(false)
    })
  })

  describe('validatePassword', () => {
    it('should validate correct password', () => {
      expect(validatePassword('abc123')).toBe(true)
      expect(validatePassword('Test123@')).toBe(true)
    })

    it('should reject invalid password', () => {
      expect(validatePassword('12345')).toBe(false)
      expect(validatePassword('abcdef')).toBe(false)
      expect(validatePassword('12345')).toBe(false)
    })
  })
})
