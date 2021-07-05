<template>
  <header>
    <a href="https://memucho.de/"
      ><img
        alt="memucho logo"
        src="../assets/Logos/LogoComplete_M.png"
        height="84"
      />
    </a>
    <p>Statistiken von memucho</p>
    <nav>
      <ul>
        <li><a href="https://memucho.de/">Home</a></li>
      </ul>
    </nav>
  </header>
</template>
<style lang="scss">
header {
  display: flex;
  border-bottom: 1px solid #ccc;
  padding: 0.5rem 1rem;
  margin: 0 !important;
  background-color: #003264;
  color: white;
  p {
    margin-left: 1rem;
  }
}

nav {
  margin-left: auto;

  ul {
    list-style: none;
  }

  ul li {
    display: inline-flex;
    margin-left: 1rem;
  }
}
</style>
