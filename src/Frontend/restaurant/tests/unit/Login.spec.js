import { shallowMount } from '@vue/test-utils'
import Login from '@/pages/Login.vue'

describe('Login page', () => {
  it('should render Login page', () => {
    const msg = 'Logowanie';
    const wrapper = shallowMount(Login)
    expect(wrapper.text()).toMatch(msg)
  })
})
